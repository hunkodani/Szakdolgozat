using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppEvaluator
{
    class Logging
    {
        private static readonly string _logFilePath = Path.Combine(Environment.CurrentDirectory, AppDomain.CurrentDomain.FriendlyName.Trim(".exe".ToCharArray()));
        private static readonly string _logFile = "log.txt";

        public static void WriteToLog(LogTypes type, string content)
        {
            try
            {
                if (!Directory.Exists(_logFilePath))
                {
                    Directory.CreateDirectory(_logFilePath);
                }
                StreamWriter stream = new StreamWriter(Path.Combine(_logFilePath, _logFile), true, Encoding.UTF8);
                stream.WriteLine(type.ToString() + ": " + content);
                stream.Close();
            }
            catch (Exception)
            {

            }
        }
    }

    public enum LogTypes {
        Log,
        Warning,
        Error
    }
}
