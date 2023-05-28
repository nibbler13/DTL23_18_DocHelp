using DTL23_API.Utilities;
using System.Data;

namespace DTL23_API.DataHandlers {
	internal static class GetListOfDoctorPositions {
		private readonly static string query = "select distinct DOCTOR_POSITION from DTL23_DATASET";

		public static List<string> GetList() {
			List<string> list = new();
			AdoClickHouseClient clickHouseClient = Program.ClickHouseClient;

			Logging.ToLog("Получение данных из БД");
			DataTable dt = clickHouseClient.GetDataTable(query);
			Logging.ToLog("Получено строк: " + dt.Rows.Count);

			foreach (DataRow dr in dt.Rows)
				list.Add(dr["DOCTOR_POSITION"].ToString());

			return list;
		}
	}
}
