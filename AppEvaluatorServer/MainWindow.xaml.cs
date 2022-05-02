using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace AppEvaluatorServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string NewDataPath { get; set; }
        public static string CurrentDataPath { get; set; }


        public bool SqlConnectionStatus
        {
            get { return (bool)GetValue(SqlConnectionStatusProperty); }
            set { SetValue(SqlConnectionStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SqlConnectionStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SqlConnectionStatusProperty =
            DependencyProperty.Register("SqlConnectionStatus", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));



        public MainWindow()
        {
            InitializeComponent();
            NetworkMethods.ListenMulticastGroup();
            NetworkMethods.ListenTcpRequests();
            SQLiteMethods.ConnectToDatabase();
            SqlConnectionStatus = SQLiteMethods.ConnectionStatus;
            try
            {
                FileMethods.LoadSettingsFromFile();
                CurrentDataPath = FileMethods.FindSettingsElement("DataRoot");
                FolderPathLbl.Content = CurrentDataPath;
                MigrationCheck.IsChecked = FileMethods.FindSettingsElement("Migration") != "False";
            }
            catch (FileNotFoundException)
            {
                CurrentDataPath = null;
            }

        }

        private void PickFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                FolderPathLbl.Content = System.IO.Path.Combine(folderBrowserDialog.SelectedPath, FileMethods.DataDirectoryName);
                NewDataPath = FolderPathLbl.Content.ToString();
                int index = FileMethods.FindSettingsElementIndex("DataRoot");
                if (index != -1)
                {
                    FileMethods.Settings[index] = new string[] { "DataRoot", NewDataPath };
                }
                else
                {
                    FileMethods.Settings.Add(new string[] { "DataRoot", NewDataPath });
                }
            }
        }

        private void SaveSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            ///create a new-old configuration so if there is a problem, it can roll back to that
            RespondLbl.Content = "";
            if (NewDataPath != null && !Directory.Exists(NewDataPath))
            {
                try
                {
                    if (MigrationCheck.IsChecked == true && CurrentDataPath != null)
                    {
                        Directory.Move(CurrentDataPath, NewDataPath);
                    }
                    else
                    {
                        _ = Directory.CreateDirectory(NewDataPath);
                    }
                    FileMethods.SaveSettingsToFile();
                    CurrentDataPath = FileMethods.FindSettingsElement("DataRoot");
                    RespondLbl.Foreground = Brushes.LimeGreen;
                    RespondLbl.Content = "Settings successfully saved";
                }
                catch (Exception)
                {
                    RespondLbl.Foreground = Brushes.DarkRed;
                    RespondLbl.Content = "An error occured while saving. Nothing changed.";
                }
            }
            else
            {
                if (NewDataPath == null)
                {
                    try
                    {
                        FileMethods.SaveSettingsToFile();
                        RespondLbl.Foreground = Brushes.LimeGreen;
                        RespondLbl.Content = "Settings successfully saved";
                    }
                    catch (Exception)
                    {
                        RespondLbl.Foreground = Brushes.DarkRed;
                        RespondLbl.Content = "An error occured while saving. Nothing changed.";
                    }

                }
                else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show(
                        "Directory already exists, would you like to use as data root anyway? Cannot migrate directory contents, only use directory as root.",
                        "Root change confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            int index = FileMethods.FindSettingsElementIndex("Migration");
                            if (index != -1)
                            {
                                FileMethods.Settings[index] = new string[] { "Migration", "False" };
                            }
                            else
                            {
                                FileMethods.Settings.Add(new string[] { "Migration", "False" });
                            }
                            FileMethods.SaveSettingsToFile();
                            CurrentDataPath = FileMethods.FindSettingsElement("DataRoot");
                            MigrationCheck.IsChecked = false;
                            RespondLbl.Foreground = Brushes.LimeGreen;
                            RespondLbl.Content = "Settings successfully saved";
                        }
                        catch (Exception)
                        {
                            RespondLbl.Foreground = Brushes.DarkRed;
                            RespondLbl.Content = "An error occured while saving. Nothing changed.";
                        }
                    }
                }
            }
        }

        private void MigrationCheck_Checked(object sender, RoutedEventArgs e)
        {
            MigrationCheckToggle();
        }

        private void MigrationCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            MigrationCheckToggle();
        }

        private void MigrationCheckToggle()
        {
            int index = FileMethods.FindSettingsElementIndex("Migration");
            if (index != -1)
            {
                FileMethods.Settings[index] = new string[] { "Migration", MigrationCheck.IsChecked.ToString() };
            }
            else
            {
                FileMethods.Settings.Add(new string[] { "Migration", MigrationCheck.IsChecked.ToString() });
            }
        }

        private void DisposeElements(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ///Aborts the Multicast listening thread by closing the sokcet
            NetworkMethods.McastSocket.Close();
            ///Aborts the Tcp listening thread
            //make separate thread and use bool world variable to stop it
            NetworkMethods.TcpServerShutdown = true;
            if (true)
            {

            }
            SQLiteMethods.DisconnectFromDatabase();
        }
    }
}
