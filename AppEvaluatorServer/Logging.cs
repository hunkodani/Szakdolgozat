using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppEvaluatorServer
{
    class Logging
    {
        private static readonly string _logFile = Path.Combine(Environment.CurrentDirectory, AppDomain.CurrentDomain.FriendlyName, "leg.txt");

        public static void WriteToLog(LogTypes type, string content)
        {
            string tmplog = "";
            try
            {
                StreamWriter stream = new StreamWriter(_logFile, true, Encoding.UTF8);
                stream.WriteLine(tmplog);
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
