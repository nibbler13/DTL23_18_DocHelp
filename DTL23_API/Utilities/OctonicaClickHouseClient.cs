using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octonica.ClickHouseClient;

namespace DTL23_API.Utilities {
    internal class OctonicaClickHouseClient {
        private readonly ClickHouseConnection connection;

        public OctonicaClickHouseClient() {
			ClickHouseConnectionStringBuilder builder = new() {
				Host = "glb-srv-dwh",
				Port = 9000,
				Database = "default",
				User = "default",
				ReadWriteTimeout = 10 * 60 * 1000,
				CommandTimeout = 10 * 60
			};

			connection = new ClickHouseConnection(builder);
            connection.Open();
        }

        public bool ExecuteNonQuery(string commandText, Dictionary<string, object>? parameters = null) {
            if (!IsConnectionOpened())
                return false;

            ClickHouseCommand command = connection.CreateCommand();
            command.CommandText = commandText;

            if (parameters != null)
                foreach (KeyValuePair<string, object> parameter in parameters)
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);

            try {
                command.ExecuteNonQuery();
                return true;
            } catch (Exception e) {
                Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
        }

        public bool BulkUpload(string command, object[] objects, int count) {
            Logging.ToLog("Загрузка данных Bulk, объектов: " + count);

            if (!IsConnectionOpened())
                return false;

            try {
                using ClickHouseColumnWriter writer = connection.CreateColumnWriter(command);
                writer.WriteTable(objects, count);
                return true;
            } catch (Exception e) {
                Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
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
    }
}
