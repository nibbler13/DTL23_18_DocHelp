using DTL23_API.DataClasses;
using DTL23_API.Utilities;
using System.Data;

namespace DTL23_API.DataHandlers {
	internal static class GetListOfDataSetServiceComparison {
		private readonly static string queryServiceComparison =
			"select ID, NAME, BATCH_APPOINTMENT_ID, " +
			"GENERAL_STANDARDS_ID, CREATE_DATETIME from DTL23_SERVICE_COMPARISON";

		private readonly static string queryGeneralStandardsServices =
			"select ID, NAME, CREATE_DATETIME " +
			"from DTL23_GENERAL_STANDARDS_SERVICES";

		private readonly static string queryBatchAppointmentsServices =
			"select ID, NAME, CREATE_DATETIME " +
			"from DTL23_BATCH_APPOINTMENTS_SERVICES";

		private static List<DataSetServiceComparison> dataSetServices;
		private static int lastHourUpdate = 0;

		public static List<DataSetServiceComparison> GetList() {
			if (lastHourUpdate == DateTime.Now.Hour &&
				dataSetServices != null)
				return dataSetServices;

			SortedDictionary<ushort, DataSetServiceComparison> dict = new();
			AdoClickHouseClient client = Program.ClickHouseClient;

			Logging.ToLog("Получение данных по сопоставлению услуг из БД");
			DataTable dtServiceComparison = client.GetDataTable(queryServiceComparison);
			Logging.ToLog("Получено строк: " + dtServiceComparison.Rows.Count);

			Logging.ToLog("Получение данных по сопоставлению услуг из БД");
			DataTable dtGeneralStandardsServices = client.GetDataTable(queryGeneralStandardsServices);
			Logging.ToLog("Получено строк: " + dtGeneralStandardsServices.Rows.Count);

			Logging.ToLog("Получение данных по сопоставлению услуг из БД");
			DataTable dtBatchAppointmentsServices = client.GetDataTable(queryBatchAppointmentsServices);
			Logging.ToLog("Получено строк: " + dtBatchAppointmentsServices.Rows.Count);

			Dictionary<ushort, string> gsServices = new();
			foreach (DataRow dr in dtGeneralStandardsServices.Rows) {
				ushort id = (ushort)dr["ID"];
				string name = (string)dr["NAME"];

				if (!gsServices.ContainsKey(id))
					gsServices.Add(id, name);
			}

			Dictionary<ushort, string> baServices = new();
			foreach (DataRow dr in dtBatchAppointmentsServices.Rows) {
				ushort id = (ushort)dr["ID"];
				string name = (string)dr["NAME"];

				if (!baServices.ContainsKey(id))
					baServices.Add(id, name);
			}

			foreach (DataRow dr in dtServiceComparison.Rows) {
				ushort id = (ushort)dr["ID"];
				string name = (string)dr["NAME"];
				ushort[] baIds = (ushort[])dr["BATCH_APPOINTMENT_ID"];
				ushort[] gsIds = (ushort[])dr["GENERAL_STANDARDS_ID"];
				DateTime createDate = (DateTime)dr["CREATE_DATETIME"];

				DataSetServiceComparison service = new() {
					Id = id,
					Name = name,
					CreateDate = createDate
				};

				foreach (ushort item in baIds)
					if (baServices.ContainsKey(item))
						service.BatchAppointmentsServices.Add(baServices[item]);

				foreach (ushort item in gsIds)
					if (gsServices.ContainsKey(item))
						service.GeneralStandardsServices.Add(gsServices[item]);

				if (!dict.ContainsKey(id))
					dict.Add(id, service);
			}

			dataSetServices = dict.Values.ToList();
			lastHourUpdate = DateTime.Now.Hour;

			return dataSetServices;
		}
	}
}
