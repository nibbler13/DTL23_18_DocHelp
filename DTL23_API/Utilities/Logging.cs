using System.ComponentModel;
using System.Reflection;

namespace DTL23_API.Utilities
{
    internal class Logging
    {
        private static readonly string LOG_FILE_NAME = Assembly.GetExecutingAssembly().GetName().Name + "_*.log";
        public static readonly string assemblyDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        private const int MAX_LOGFILES_QUANTITY = 31;

        public static void ToLog(string msg)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            string logFileName = assemblyDirectory + LOG_FILE_NAME.Replace("*", today);

            try
            {
                using (StreamWriter sw = File.AppendText(logFileName))
                {
                    string logLine = string.Format("{0:G}: {1}", DateTime.Now, msg);
                    sw.WriteLine(logLine);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("LogMessageToFile exception: " + logFileName + Environment.NewLine + e.Message +
                    Environment.NewLine + e.StackTrace);
            }

            Console.WriteLine(msg);
            CheckAndCleanOldFiles();
        }

        private static void CheckAndCleanOldFiles()
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                FileInfo[] files = dirInfo.GetFiles(LOG_FILE_NAME).OrderBy(p => p.CreationTime).ToArray();

                if (files.Length <= MAX_LOGFILES_QUANTITY)
                    return;

                for (int i = 0; i < files.Length - MAX_LOGFILES_QUANTITY; i++)
                    files[i].Delete();

            }
            catch (Exception e)
            {
                Console.WriteLine("CheckAndCleanOldFiles exception" + e.Message + Environment.NewLine + e.StackTrace);
            }
        }
    }
}
