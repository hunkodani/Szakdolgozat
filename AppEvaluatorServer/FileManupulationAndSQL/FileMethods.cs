using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppEvaluatorServer.FileManupulationAndSQL
{
    internal static class FileMethods
    {
        #region Settings

        public static string DataRoot { get; set; }
        internal static string SettingsFilePath
        {
            get
            {
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string path = Environment.CurrentDirectory;
                //path = Path.Combine(path, AppDomain.CurrentDomain.FriendlyName);
                path = Path.Combine(path, System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                return path;
            }
        }
        private static string SettingsFile => Path.Combine(SettingsFilePath, "settings.txt");
        public static string DataDirectoryName => "AppEvaluator_Data";

        public static List<string[]> Settings = new List<string[]>();

        public static void SaveSettingsToFile()
        {
            List<string> tmp = new List<string>();
            Settings.ForEach(item => tmp.Add(item[0] + "=" + item[1]));
            if (File.Exists(SettingsFile))
            {
                File.WriteAllLines(SettingsFile, tmp);
            }
            else
            {
                File.Create(SettingsFile).Close();
                File.WriteAllLines(SettingsFile, tmp);
            }
        }

        public static void LoadSettingsFromFile()
        {
            Settings.Clear();
            if (File.Exists(SettingsFile))
            {
                List<string> tmp = File.ReadAllLines(SettingsFile).ToList();
                foreach (string item in tmp)
                {
                    Settings.Add(item.Split('='));
                }
            }
            else
            {
                if (!Directory.Exists(SettingsFilePath))
                {
                    _ = Directory.CreateDirectory(SettingsFilePath);
                    File.Create(SettingsFile).Close();
                }
                throw new FileNotFoundException("No settings file was found. Settings file created.");
            }
        }

        public static int FindSettingsElementIndex(string value)
        {
            return Settings.FindIndex(item => item.ElementAt(0) == value);
        }

        public static string FindSettingsElement(string value)
        {
            string[] tmp = Settings.Find(item => item.ElementAt(0) == value);
            if (tmp == null)
            {
                return null;
            }
            return tmp.ElementAt(1);
        }

        #endregion

    }
}
