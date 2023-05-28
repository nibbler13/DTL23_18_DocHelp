using ClickHouseETL.Services;
using DTL23_API.DataClasses;
using DTL23_API.Utilities;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace DTL23_API.DataHandlers {
	internal static class UploadFileToDb {
		public static UploadFileResult ParseAndUploadFileToDb(string fileName, string realFileName, string targetFilePath) {
			UploadFileResult result = new() { FileName = realFileName };

			string filePath = Path.Combine(targetFilePath, fileName);
			Logging.ToLog("Считывание данных из файла: " + filePath);

			List<string> sheetNames = ExcelReader.ReadSheetNamesNpoi(filePath);
			if (sheetNames.Count == 0) {
				result.MessageToDisplay = "Не удалось считать список листов в файле";
				return result;
			}

			Logging.ToLog("Лист: " + sheetNames[0]);
			DataTable dt = ExcelReader.ReadExcelFileNpoi(filePath, sheetNames[0]);
			if (dt == null) {
				result.MessageToDisplay = "Не удалось считать данные из файла";
				return result;
			}

			if (dt.Rows.Count < 2) {
				result.MessageToDisplay = "Файл не содержит данных для загрузки (строк менее 2)";
				return result;
			}

			Dictionary<string, int> columnsInLoadedFile = new() {
				{ "пол пациента", -1 },
				{ "дата рождения пациента", -1 },
				{ "id пациента", -1 },
				{ "код мкб-10", -1 },
				{ "диагноз", -1 },
				{ "дата оказания услуги", -1 },
				{ "должность", -1 },
				{ "назначения", -1 }
			};

			for (int i = 0; i < dt.Rows[0].ItemArray.Length; i++) {
				object value = dt.Rows[0].ItemArray[i];
				if (value == null || string.IsNullOrEmpty(value.ToString()))
					continue;

				string str = value.ToString().ToLower().TrimStart(' ').TrimEnd(' ');

				if (!columnsInLoadedFile.ContainsKey(str))
					continue;

				columnsInLoadedFile[str] = i;
			}

			bool isHeadersCorrect = true;
			string headersWithError = string.Empty;
			foreach (KeyValuePair<string, int> pair in columnsInLoadedFile) {
				if (pair.Value != -1)
					continue;

				isHeadersCorrect = false;
				headersWithError += pair.Key + "; ";
			}

			if (!isHeadersCorrect) {
				headersWithError = headersWithError.TrimEnd(' ').TrimEnd(',');
				result.MessageToDisplay = "Не удалось определить положение " +
					"заголовков в первой строке: " + headersWithError;
				return result;
			}

			Dictionary<string, List<object>> parametersToLoadInDb = new() {
				{ "ID", new() },
				{ "SEX", new() },
				{ "BIRTHDAY", new() },
				{ "CLIENT_ID", new() },
				{ "MKB", new() },
				{ "DIAGNOSIS", new() },
				{ "TREAT_DATE", new() },
				{ "DOCTOR_POSITION", new() },
				{ "REFERRALS", new() },
				{ "CREATE_DATETIME", new() }
			};

			Logging.ToLog("Загрузка данных по DTL23DataSet в ClickHouse");
			OctonicaClickHouseClient clickHouseClient = new();

			string query = "insert into DTL23_DATASET (" +
				"ID, SEX, BIRTHDAY, CLIENT_ID, MKB, DIAGNOSIS, TREAT_DATE, DOCTOR_POSITION, " +
				"REFERRALS, CREATE_DATETIME) values";

			bool isResultOk = true;

			for (int i = 1; i < dt.Rows.Count; i++) {
				DataRow dr = dt.Rows[i];

				string sex = dr.ItemArray[columnsInLoadedFile["пол пациента"]].ToString();
				string birthday = dr.ItemArray[columnsInLoadedFile["дата рождения пациента"]].ToString();
				string clientId = dr.ItemArray[columnsInLoadedFile["id пациента"]].ToString();
				string mkb = dr.ItemArray[columnsInLoadedFile["код мкб-10"]].ToString();
				string diagnosis = dr.ItemArray[columnsInLoadedFile["диагноз"]].ToString();
				string treatDate = dr.ItemArray[columnsInLoadedFile["дата оказания услуги"]].ToString();
				string doctorPosition = dr.ItemArray[columnsInLoadedFile["должность"]].ToString();
				string referrals = dr.ItemArray[columnsInLoadedFile["назначения"]].ToString();
				string id = sex + birthday + clientId + mkb + diagnosis + treatDate + doctorPosition + referrals;
				bool isBdateParsed = DateTime.TryParse(birthday, out DateTime bdate);
				bool isTdateParsed = DateTime.TryParse(treatDate, out DateTime tdate);

				if (!string.IsNullOrEmpty(id) && isBdateParsed && isTdateParsed) {
					id = Helpers.MD5.CreateMD5(id);
					parametersToLoadInDb["ID"].Add(id);
					parametersToLoadInDb["SEX"].Add(sex);
					parametersToLoadInDb["BIRTHDAY"].Add(DateOnly.FromDateTime(bdate));
					parametersToLoadInDb["CLIENT_ID"].Add(clientId);
					parametersToLoadInDb["MKB"].Add(mkb);
					parametersToLoadInDb["DIAGNOSIS"].Add(diagnosis);
					parametersToLoadInDb["TREAT_DATE"].Add(DateOnly.FromDateTime(tdate));
					parametersToLoadInDb["DOCTOR_POSITION"].Add(doctorPosition);
					parametersToLoadInDb["REFERRALS"].Add(referrals);
					parametersToLoadInDb["CREATE_DATETIME"].Add(new DateTimeOffset(DateTime.Now));
					result.RowsReaded++;

				} else if (!isBdateParsed)
					result.RowsWithBrokenBirthdayDate.Add(i + 1);

				else if (!isTdateParsed)
					result.RowsWithBrokenTreatDate.Add(i + 1);

				if (parametersToLoadInDb["ID"].Count < 1000 && i != dt.Rows.Count - 1)
					continue;
				
				if (parametersToLoadInDb["ID"].Count > 0)
					isResultOk &= clickHouseClient.BulkUpload(
						query,
						parametersToLoadInDb.Values.ToArray(),
						parametersToLoadInDb["ID"].Count);

				result.RowsUploaded += parametersToLoadInDb["ID"].Count;

				foreach (KeyValuePair<string, List<object>> item in parametersToLoadInDb)
					item.Value.Clear();
			}

			clickHouseClient.ExecuteNonQuery("optimize table DTL23_DATASET");

			if (isResultOk) {
				result.MessageToDisplay = "Данные успешно загружены в БД";
				result.IsLoaded = true;
			} else
				result.MessageToDisplay = "Не удалось записать данные в БД";

			return result;
		}
	}
}
