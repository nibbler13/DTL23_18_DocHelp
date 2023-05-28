using DTL23_API.Utilities;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseETL.Services {
	internal class ExcelReader {

		public static List<string> ReadSheetNamesNpoi(string fileName) {
			List<string> sheetNames = new List<string>();
			IWorkbook workbook;

			try {
				FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				workbook = fileName.Contains(".xlsx") ? new XSSFWorkbook(fs) : (IWorkbook)new HSSFWorkbook(fs);

				if (workbook == null)
					return sheetNames;

				for (int i = 0; i < workbook.NumberOfSheets; i++) {
					string sheetName = workbook.GetSheetName(i);
					if (!string.IsNullOrEmpty(sheetName))
						sheetNames.Add(sheetName);
				}
			} catch (Exception e) {
				Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
			}

			return sheetNames;
		}

		public static DataTable ReadExcelFileNpoi(string fileName,
										   string sheetName) {
			IWorkbook workbook;
			ISheet sheet;
			DataTable dt = new DataTable();

			try {
				FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
				workbook = fileName.Contains(".xlsx") ? new XSSFWorkbook(fs) : (IWorkbook)new HSSFWorkbook(fs);

				if (sheetName != null) {
					sheet = workbook.GetSheet(sheetName);

					if (sheet == null)
						sheet = workbook.GetSheetAt(0);
				} else
					sheet = workbook.GetSheetAt(0);

				if (sheet == null) 
					return dt;

				for (int currentRow = 0; currentRow < sheet.LastRowNum; currentRow++) {
					IRow row = sheet.GetRow(currentRow);

					if (row != null && dt.Columns.Count < row.LastCellNum)
						for (int i = dt.Columns.Count; i < row.LastCellNum; i++)
							dt.Columns.Add(new DataColumn());

					DataRow dataRow = dt.NewRow();

					if (row != null)
						for (int currentColumn = 0; currentColumn < row.LastCellNum; currentColumn++) {
							ICell currentCell = row.GetCell(currentColumn);
							if (currentCell != null) {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
								string value = currentCell.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

								if (currentCell.CellType == CellType.Numeric)
									if (DateUtil.IsCellDateFormatted(currentCell)) {
										DateTime dateTime = currentCell.DateCellValue;
										value = currentCell.NumericCellValue == (int)currentCell.NumericCellValue
											? dateTime.ToString("dd.MM.yyyy")
											: dateTime.ToString("dd.MM.yyyy HH:mm:ss");
									}

								dataRow[currentColumn] = value;
							}
						}

					dt.Rows.Add(dataRow);
				}

				return dt;
			} catch (Exception e) {
				Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
#pragma warning disable CS8603 // Possible null reference return.
				return null;
#pragma warning restore CS8603 // Possible null reference return.
			}
		}

	}
}
