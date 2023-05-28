using DTL23_API.DataClasses;

namespace DTL23_API.DataHandlers {
	internal static class GetDashboardData {
		public static DashboardData GetData(DateTime dateBegin,
									  DateTime dateEnd,
									  string? doctorPostion,
									  string? mkbCode) {
			DashboardData dashboardData = new();

			List<DetailedInfo> infoList = GetListOfDetailedInfo.GetList(
				dateBegin,
				dateEnd,
				mkbCode,
				doctorPostion);

			foreach (DetailedInfo info in infoList) {
				dashboardData.TotalTreatmentsCount++;
				dashboardData.TotalReferralsCount += (uint)info.Referrals.Count;

				if (info.TreatmentHasStandards)
					dashboardData.TreatmentsWithStandardsCount++;
				else
					dashboardData.TreatmentsWithoutStandardsCount++;

				switch (info.StatusType) {
					case DetailedInfo.ReferralsStatusType.TreatmentHasNoReferrals:
						dashboardData.TreatmentsStatusHasNoReferrals++;
						break;

					case DetailedInfo.ReferralsStatusType.AllInStandards:
						dashboardData.TreatmentsStatusAllInStandards++;
						break;

					case DetailedInfo.ReferralsStatusType.PartlyInStandards:
						dashboardData.TreatmentsStatusPartlyInStandards++;
						break;

					case DetailedInfo.ReferralsStatusType.NoneInStandards:
						dashboardData.TreatmentsStatusNoneInStandards++;
						break;

					default:
						break;
				}

				foreach (DataSetReferral referral in info.Referrals) {
					if (info.TreatmentHasStandards)
						dashboardData.ReferralsCountWithStandards++;
					else {
						dashboardData.ReferralsCountWithoutStandards++;
						continue;
					}

					if (!referral.IsPresentsInStandards) {
						dashboardData.ReferralsOutsideStandards++;
						continue;
					}

					if (referral.IsServiceNecessary ?? false)
						dashboardData.ReferralsHasComparedNecessary++;
					else
						dashboardData.ReferralsHasComparedOptional++;
				}
			}

			return dashboardData;
		}
	}
}
