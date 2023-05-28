using DTL23_API.DataClasses;
using DTL23_API.Utilities;
using System.Data;

namespace DTL23_API.DataHandlers {
	internal static class GetListOfMkbCodes {
		private readonly static string query = "select distinct MKB, DIAGNOSIS from DTL23_DATASET";

		public static List<MkbCode> GetList() {
			List<MkbCode> list = new();
			AdoClickHouseClient clickHouseClient = Program.ClickHouseClient;

			Logging.ToLog("Получение данных из БД");
			DataTable dt = clickHouseClient.GetDataTable(query);
			Logging.ToLog("Получено строк: " + dt.Rows.Count);

			foreach (DataRow dr in dt.Rows)
				list.Add(new MkbCode { Mkb = (string)dr["MKB"], Diagnosis = (string)dr["DIAGNOSIS"] });

			return list;
		}
	}
}
