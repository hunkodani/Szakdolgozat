using AppEvaluatorServer.Commands;
using AppEvaluatorServer.FileManupulationAndSQL;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluatorServer.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public string NewDataPath { get; set; }

        public ICommand PickRootFolderCmd { get; }
        public ICommand ToggleMigrationCmd { get; }
        public ICommand ToggleMulticastCmd { get; }
        public ICommand SaveSettingsCmd { get; }


        private bool _sqlConnectionStatus;

        public bool SqlConnectionStatus
        {
            get { return _sqlConnectionStatus; }
            set 
            { 
                _sqlConnectionStatus = value; 
                OnPropertyChanged(nameof(SqlConnectionStatus));
            }
        }

        private string _folderPathLbl;

        public string FolderPathLbl
        {
            get { return _folderPathLbl; }
            set 
            { 
                _folderPathLbl = value; 
                OnPropertyChanged(nameof(FolderPathLbl));
            }
        }

        private bool _isMigrate = true;

        public bool IsMigrate
        {
            get { return _isMigrate; }
            set
            {
                _isMigrate = value;
                OnPropertyChanged(nameof(IsMigrate));
            }
        }

        private bool _multicastStatus;

        public bool MulticastStatus
        {
            get { return _multicastStatus; }
            set
            {
                _multicastStatus = value;
                OnPropertyChanged(nameof(MulticastStatus));
                OnPropertyChanged(nameof(MulticastIP));
                OnPropertyChanged(nameof(MulticastVisibility));
            }
        }

        public string MulticastIP => WcfServicesAndNetworking.NetworkMethods.McastIPAddress.ToString();

        public Visibility MulticastVisibility => MulticastStatus == true ? Visibility.Visible : Visibility.Hidden;


        #region MessagesAndColors

        private string _saveMsg;

        public string SaveMsg
        {
            get { return _saveMsg; }
            set
            {
                _saveMsg = value;
                OnPropertyChanged(nameof(SaveMsg));
            }
        }

        private Brush _saveMsgColor;

        public Brush SaveMsgColor
        {
            get { return _saveMsgColor; }
            set
            {
                _saveMsgColor = value;
                OnPropertyChanged(nameof(SaveMsgColor));
            }
        }

        private string _errorMsg;

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set
            {
                _errorMsg = value;
                OnPropertyChanged(nameof(ErrorMsg));
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            PickRootFolderCmd = new PickRootFolderCmd(this);
            ToggleMigrationCmd = new ToggleMigrationCmd(this);
            ToggleMulticastCmd = new ToggleMulticastCmd(this);
            SaveSettingsCmd = new SaveSettingsCmd(this);

            SqlConnectionStatus = SQLiteMethods.ConnectionStatus;
            try
            {
                if (Properties.Settings.Default.IsFirstStart)
                {
                    NewDataPath = Path.Combine(Environment.CurrentDirectory, FileMethods.DataDirectoryName);
                    FolderPathLbl = NewDataPath;
                    FirstStart();
                    Properties.Settings.Default.IsFirstStart = false;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    FileMethods.LoadSettingsFromFile();
                    FileMethods.DataRoot = FileMethods.FindSettingsElement("DataRoot");
                    FolderPathLbl = FileMethods.DataRoot;
                    IsMigrate = FileMethods.FindSettingsElement("Migration") != "False";
                    UpdateMulticasting();
                }
            }
            catch (FileNotFoundException e)
            {
                FileMethods.DataRoot = null;
                Logging.WriteToLog(LogTypes.Error, e.Message);
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                FileMethods.DataRoot = null;
                Logging.WriteToLog(LogTypes.Error, e.Message);
            }
        }

        /// <summary>
        /// Creates a folder root at the first start and saves the settings file
        /// </summary>
        private void FirstStart()
        {
            SaveMsg = "";
            try
            {
                if (!Directory.Exists(NewDataPath))
                {
                    _ = Directory.CreateDirectory(NewDataPath);
                    _ = Directory.CreateDirectory(NewDataPath + "\\Subjects");
                    _ = Directory.CreateDirectory(NewDataPath + "\\Users");
                }
                FileMethods.Settings.Add(new string[] { "DataRoot", NewDataPath });
                FileMethods.SaveSettingsToFile();
                FileMethods.DataRoot = FileMethods.FindSettingsElement("DataRoot");
            }
            catch (Exception)
            {
                SaveMsgColor = Brushes.DarkRed;
                SaveMsg = "An error occured while saving. Nothing everything was saved.";
            }
        }

        /// <summary>
        /// Updates the multicasting setting
        /// </summary>
        public void UpdateMulticasting()
        {
            string tmp = FileMethods.FindSettingsElement("Multicasting");
            if (tmp == null || tmp == "True")
            {
                WcfServicesAndNetworking.NetworkMethods.ListenMulticastGroup();
                MulticastStatus = true;
            }
            else
            {
                WcfServicesAndNetworking.NetworkMethods.McastSocket.Close();
                if (WcfServicesAndNetworking.NetworkMethods.IsMulticasting)
                {
                    WcfServicesAndNetworking.NetworkMethods.IsMulticasting = false;
                }
                MulticastStatus = false;
            }
        }
    }
}
