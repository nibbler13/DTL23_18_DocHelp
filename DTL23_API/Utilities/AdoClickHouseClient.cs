using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Client.ADO;

namespace DTL23_API.Utilities {
	internal class AdoClickHouseClient {
		private readonly ClickHouseConnection connection;

		public AdoClickHouseClient() {
			ClickHouseConnectionStringBuilder cs = new ClickHouseConnectionStringBuilder {
				Host = "glb-srv-dwh",
				Database = "default",
				Username = "default"
			};

			connection = new ClickHouseConnection(cs.ToString());
			IsConnectionOpened();
		}

		private bool IsConnectionOpened() {
			if (connection.State != ConnectionState.Open) {
				try {
					connection.Open();
				} catch (Exception e) {
					string subject = "Ошибка подключения к БД";
					string body = e.Message + Environment.NewLine + e.StackTrace;
					Logging.ToLog(subject + " " + body);
				}
			}

			return connection.State == ConnectionState.Open;
		}

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		public DataTable GetDataTable(string query, Dictionary<string, object> parameters = null) {
			DataTable dataTable = new DataTable();

			if (!IsConnectionOpened())
				return dataTable;

			try {
				using (ClickHouseCommand command = connection.CreateCommand()) {
					if (parameters != null && parameters.Count > 0)
						foreach (KeyValuePair<string, object> parameter in parameters) {
							if (parameter.Value is int ||
								parameter.Value is long ||
								parameter.Value is float ||
								parameter.Value is double)
								query = query.Replace(parameter.Key, parameter.Value.ToString());
							else
								query = query.Replace(parameter.Key, "'" + 
									parameter.Value.ToString() + "'");
						}

					command.CommandText = query;

					using (ClickHouse.Client.ADO.Adapters.ClickHouseDataAdapter fbDataAdapter =
						new ClickHouse.Client.ADO.Adapters.ClickHouseDataAdapter()) {
						fbDataAdapter.SelectCommand = command;
						fbDataAdapter.Fill(dataTable);
					}
				}
			} catch (Exception e) {
				string subject = "Ошибка выполнения запроса к БД";
				string body = e.Message + Environment.NewLine + e.StackTrace;
				Logging.ToLog(subject + " " + body);
				connection.Close();
			}

			return dataTable;
		}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
	}
}
