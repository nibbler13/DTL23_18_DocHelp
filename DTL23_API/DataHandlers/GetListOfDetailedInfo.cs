using DTL23_API.DataClasses;
using DTL23_API.Utilities;
using System.Data;

namespace DTL23_API.DataHandlers {
	internal static class GetListOfDetailedInfo {
		private readonly static string query =
			"select ID, SEX, BIRTHDAY, CLIENT_ID, MKB, " +
			"DIAGNOSIS, TREAT_DATE, DOCTOR_POSITION, REFERRALS, CREATE_DATETIME " +
			"from DTL23_DATASET where TREAT_DATE between '@dateBegin' and '@dateEnd'";

		public static List<DetailedInfo> GetList(DateTime dateBegin,
										   DateTime dateEnd,
										   string? mkbCode = null,
										   string? doctorPosition = null) {
			List<DetailedInfo> list = new();
			string queryToRun = query.
				Replace("@dateBegin", dateBegin.ToString("yyyy.MM.dd")).
				Replace("@dateEnd", dateEnd.ToString("yyyy.MM.dd"));

			if (!string.IsNullOrEmpty(mkbCode))
				queryToRun += " and MKB = '" + mkbCode + "'";

			if (!string.IsNullOrEmpty(doctorPosition))
				queryToRun += " and DOCTOR_POSITION = '" + doctorPosition + "'";

			AdoClickHouseClient clickHouseClient = Program.ClickHouseClient;

			Logging.ToLog("Получение данных из БД");
			DataTable dt = clickHouseClient.GetDataTable(queryToRun);
			Logging.ToLog("Получено строк: " + dt.Rows.Count);

			List<DataSetServiceComparison> comparison = GetListOfDataSetServiceComparison.GetList();

			foreach (DataRow dr in dt.Rows) {
				string mkb = dr["MKB"].ToString();

				DetailedInfo info = new() {
					Id = (string)dr["ID"],
					Sex = dr["SEX"].ToString(),
					BirthdayDate = (DateTime)dr["BIRTHDAY"],
					ClientId = dr["CLIENT_ID"].ToString(),
					Mkb = mkb,
					Diagnosis = (string)dr["DIAGNOSIS"],
					TreatDate = (DateTime)dr["TREAT_DATE"],
					DoctorPosition = (string)dr["DOCTOR_POSITION"],
					CreateDateTime = (DateTime)dr["CREATE_DATETIME"]
				};

				list.Add(info);

				List<BatchAppointment> batchAppointmentList = GetListOfBatchAppointments.GetList(mkb);
				List<GeneralStandard> generalStandardsList = GetListOfGeneralStandards.GetList(mkb);

				if (batchAppointmentList.Count == 0 &&
					generalStandardsList.Count == 0) {
					info.TreatmentHasStandards = false;
					info.StatusType = DetailedInfo.ReferralsStatusType.TreatmentHasNoStandards;
				} else  
					info.TreatmentHasStandards = true;

				string referralsArray = (string)dr["REFERRALS"];

				if (string.IsNullOrEmpty(referralsArray)) {
					info.StatusType = DetailedInfo.ReferralsStatusType.TreatmentHasNoReferrals;
					continue;
				}

				string[] referralsArraySplitted =
						referralsArray.Split("\n", StringSplitOptions.RemoveEmptyEntries);

				//Перебор назначений разделенных по строкам
				foreach (string referralArray in referralsArraySplitted) {
					string[] referralSptitted = referralArray.Split(';', StringSplitOptions.RemoveEmptyEntries);

					//Перебор услуг в назначениях, разделенных через ;
					foreach (string referral in referralSptitted) {
						string referralCleaned = referral.TrimStart(' ').TrimEnd(' ');

						DataSetReferral dataSetReferral = new () { Name = referralCleaned };
						info.Referrals.Add(dataSetReferral);

						if (batchAppointmentList.Count == 0 &&
							generalStandardsList.Count == 0)
							continue;

						//Получение списка сопоставлений услуги со стандартами
						List<DataSetServiceComparison> referralComparisons =
							comparison.Where(x => x.Name.Equals(referralCleaned)).ToList();

						//Если не удалось найти подготовленное сопоставление
						if (referralComparisons.Count == 0) {
							IsServiceComparisonFound(
								referralCleaned,
								batchAppointmentList,
								generalStandardsList,
								ref dataSetReferral);

						} else { //Есть подготовленные сопоставления
							bool isServiceFound = false;

							//Перебор услуг в сопоставлениях
							foreach (DataSetServiceComparison referralComparison in referralComparisons) {
								//Перебор сопоставлений среди пакетных назначений
								foreach (string baService in referralComparison.BatchAppointmentsServices) {
									if (!IsServiceComparisonFound(
										baService,
										batchAppointmentList,
										generalStandardsList,
										ref dataSetReferral))
										continue;

									isServiceFound = true;
									break;
								}

								if (isServiceFound)
									break;

								//Перебор сопоставлений среди приказов минздрава
								foreach (string gsService in referralComparison.GeneralStandardsServices) {
									if (!IsServiceComparisonFound(
										gsService,
										batchAppointmentList,
										generalStandardsList,
										ref dataSetReferral))
										continue;

									isServiceFound = true;
									break;
								}

								if (isServiceFound)
									break;
							}
						}
					}
				}

				int totalReferrals = info.Referrals.Count;
				if (totalReferrals == 0) {
					info.StatusType = DetailedInfo.ReferralsStatusType.TreatmentHasNoReferrals;
					continue;
				}

				if (!info.TreatmentHasStandards)
					continue;

				int referralInStandards = 0;

				foreach (DataSetReferral referral in info.Referrals)
					if (referral.IsPresentsInStandards)
						referralInStandards++;

				if (referralInStandards == 0) {
					info.StatusType = DetailedInfo.ReferralsStatusType.NoneInStandards;
					continue;
				}

				if (referralInStandards == totalReferrals) {
					info.StatusType = DetailedInfo.ReferralsStatusType.AllInStandards;
					continue;
				}

				info.StatusType = DetailedInfo.ReferralsStatusType.PartlyInStandards;
			}

			return list;
		}

		private static bool IsServiceComparisonFound(string name,
											List<BatchAppointment> batchAppointments,
											List<GeneralStandard> generalStandards,
											ref DataSetReferral referral) {
			name = name.ToUpper();

			//Сначала проверка по пакетным назначениям
			foreach (BatchAppointment ba in batchAppointments)
				foreach (BatchAppointmentService bas in ba.Services.Values) {
					if (!name.Equals(bas.Name.ToUpper()))
						continue;

					referral.IsPresentsInStandards = true;
					referral.AssociatedStandardType = DataSetReferral.StandardType.BatchAppointment;
					referral.AssociatedStandardsName = ba.Section + " | " + ba.Group + " | " + ba.Mkb;
					referral.AssociatedServiceId = bas.DbRowId;
					referral.AssociatedServiceName = bas.Name;
					referral.IsServiceNecessary = bas.Necessity.ToUpper().Equals("ДА");
					return true;
				}

			//Если не найдено в пакетных назначениях, ищем в приказах минздрава
			foreach (GeneralStandard gs in generalStandards)
				foreach (GeneralStandardsSection gss in gs.Sections.Values)
					foreach (GeneralStandardsServicesGroup gssg in gss.ServicesGroups.Values)
						foreach (GeneralStandardService gsService in gssg.Services.Values) {
							if (!name.Equals(gsService.Name.ToUpper()))
								continue;

							referral.IsPresentsInStandards = true;
							referral.AssociatedStandardType = DataSetReferral.StandardType.GeneralStandard;
							referral.AssociatedStandardsName = gs.Name + " | " + gs.MkbGroup;
							referral.AssociatedServiceId = gsService.DbRowId;
							referral.AssociatedServiceName = gsService.Name;
							referral.IsServiceNecessary = gsService.ApplicationFrequencyIndex >= 1.0f;
							return true;
						}

			return false;
		}
	}
}
