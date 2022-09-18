using System;
using System.Text;
using System.IO;

namespace AppEvaluatorServer.FileManupulationAndSQL
{
    class Logging
    {
        private static readonly string _logFile = Path.Combine(FileMethods.SettingsFilePath, "log.txt");

        public static void WriteToLog(LogTypes type, string content)
        {
            try
            {
                StreamWriter stream = new StreamWriter(_logFile, true, Encoding.UTF8);
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
