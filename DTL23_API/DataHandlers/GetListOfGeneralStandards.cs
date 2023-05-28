using DTL23_API.DataClasses;
using DTL23_API.Utilities;
using System.Data;

namespace DTL23_API.DataHandlers {
	internal static class GetListOfGeneralStandards {
		private readonly static string query = 
			"select ID, ORDER_NAME, GROUP, MKB, SECTION, TYPE, SERVICE_CODE, " +
			"SERVICE_NAME, APPLICATION_FREQUENCY_INDEX, RATE_OF_SUBMITTING, " +
			"COMMENT, CREATE_DATETIME from DTL23_GENERAL_STANDARDS";

		private static List<GeneralStandard> generalStandards;
		private static int lastHourUpdate = 0;

		public static List<GeneralStandard> GetList(string? mkbCode) {
			if (lastHourUpdate == DateTime.Now.Hour &&
				generalStandards != null &&
				string.IsNullOrEmpty(mkbCode))
				return generalStandards;

			SortedDictionary<string, GeneralStandard> dict = new();
			AdoClickHouseClient client = Program.ClickHouseClient;
			string queryToRun = query;

			if (!string.IsNullOrEmpty(mkbCode))
				queryToRun += " where MKB like '%" + mkbCode + "%'";

			Logging.ToLog("Получение данных из БД");
			DataTable dt = client.GetDataTable(queryToRun);
			Logging.ToLog("Получено строк: " + dt.Rows.Count);

			foreach (DataRow dr in dt.Rows) {
				string id = (string)dr["ID"];
				string orderName = (string)dr["ORDER_NAME"];
				string purpose = (string)dr["GROUP"];
				string mkbGroup = (string)dr["MKB"];
				string section = (string)dr["SECTION"];
				string group = (string)dr["TYPE"];
				string serviceCode = (string)dr["SERVICE_CODE"];
				string serviceName = (string)dr["SERVICE_NAME"];
				float applicationFrequencyIndex = (float)dr["APPLICATION_FREQUENCY_INDEX"];
				float rateOfSubmitting = (float)dr["RATE_OF_SUBMITTING"];
				string description = (string)dr["COMMENT"];

				if (!dict.ContainsKey(orderName))
					dict.Add(
						orderName,
						new() {
							Name = orderName,
							Purpose = purpose,
							Description = description,
							MkbGroup = mkbGroup
						}
					);

				GeneralStandard gs = dict[orderName];
				if (!gs.Sections.ContainsKey(section))
					gs.Sections.Add(section, new() { SectionName = section });

				if (!gs.Sections[section].ServicesGroups.ContainsKey(group))
					gs.Sections[section].ServicesGroups.Add(
						group, new() { GroupName = group });

				GeneralStandardService service = new() {
					DbRowId = id,
					Code = serviceCode,
					Name = serviceName,
					ApplicationFrequencyIndex = applicationFrequencyIndex,
					RateOfSubmitting = rateOfSubmitting,
				};

				if (gs.Sections[section].ServicesGroups[group].Services.ContainsKey(serviceName)) {
					GeneralStandardService serviceExisted =
						gs.Sections[section].ServicesGroups[group].Services[serviceName];
					serviceExisted.DbRowId += ", " + id;
					serviceExisted.ApplicationFrequencyIndex += applicationFrequencyIndex;
					serviceExisted.RateOfSubmitting += rateOfSubmitting;
				} else
					gs.Sections[section].ServicesGroups[group].Services.Add(serviceName, service);
			}

			if (string.IsNullOrEmpty(mkbCode)) {
				generalStandards = dict.Values.ToList();
				lastHourUpdate = DateTime.Now.Hour;
			}

			return dict.Values.ToList();
		}
	}
}
