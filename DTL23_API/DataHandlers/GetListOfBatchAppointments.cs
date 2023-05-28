using DTL23_API.DataClasses;
using DTL23_API.Utilities;
using System.Data;

namespace DTL23_API.DataHandlers {
	internal static class GetListOfBatchAppointments {
		private readonly static string query = 
			"select ID, SECTION, GROUP, MKB, ORDER, TYPE, SERVICE, " +
			"NECESSITY, COMMENT, CREATE_DATETIME from DTL23_BATCH_APPOINTMENTS";

		private static List<BatchAppointment> batchAppointments;
		private static int lastHourUpdate = 0;

		public static List<BatchAppointment> GetList(string? mkbCode) {
			if (lastHourUpdate == DateTime.Now.Hour &&
				batchAppointments != null &&
				string.IsNullOrEmpty(mkbCode))
				return batchAppointments;

			SortedDictionary<string, BatchAppointment> dict = new();
			AdoClickHouseClient client = Program.ClickHouseClient;
			string queryToRun = query;

			if (!string.IsNullOrEmpty(mkbCode))
				queryToRun += " where MKB like '%" + mkbCode + "%'";

			Logging.ToLog("Получение данных из БД");
			DataTable dt = client.GetDataTable(queryToRun);
			Logging.ToLog("Получено строк: " + dt.Rows.Count);

			foreach (DataRow dr in dt.Rows) {
				string id = (string)dr["ID"];
				string section = (string)dr["SECTION"];
				string group = (string)dr["GROUP"];
				string mkb = (string)dr["MKB"];
				short order = (short)dr["ORDER"];
				string type = (string)dr["TYPE"];
				string name = (string)dr["SERVICE"];
				string necessity = (string)dr["NECESSITY"];
				string comment = (string)dr["COMMENT"];

				string key = section + group;
				if (!dict.ContainsKey(key))
					dict.Add(key, new() {
						Section = section,
						Group = group,
						Mkb = mkb
					});

				BatchAppointmentService service = new() {
					DbRowId = id,
					Order = order,
					Type = type,
					Name = name,
					Necessity = necessity,
					Comment = comment
				};

				if (!dict[key].Services.ContainsKey(order))
					dict[key].Services.Add(order, service);
			}

			if (string.IsNullOrEmpty(mkbCode)) {
				batchAppointments = dict.Values.ToList();
				lastHourUpdate = DateTime.Now.Hour;
			}

			return dict.Values.ToList();
		}
	}
}
