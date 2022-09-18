using AppEvaluatorServer.FileManupulationAndSQL;
using AppEvaluatorServer.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluatorServer.Commands
{
    internal class SaveSettingsCmd : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public SaveSettingsCmd(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override void Execute(object parameter)
        {
            SaveSettings();
        }

        public void SaveSettings()
        {
            ///should create a new-old configuration so if there is a problem, it can roll back to that
            _mainWindowViewModel.SaveMsg = "";
            if (_mainWindowViewModel.NewDataPath != null && !Directory.Exists(_mainWindowViewModel.NewDataPath))
            {
                try
                {
                    if (_mainWindowViewModel.IsMigrate == true && FileMethods.DataRoot != null)
                    {
                        Directory.Move(FileMethods.DataRoot, _mainWindowViewModel.NewDataPath);
                    }
                    else
                    {
                        _ = Directory.CreateDirectory(_mainWindowViewModel.NewDataPath);
                        _ = Directory.CreateDirectory(_mainWindowViewModel.NewDataPath + "\\Subjects");
                        _ = Directory.CreateDirectory(_mainWindowViewModel.NewDataPath + "\\Users");
                    }
                    FileMethods.SaveSettingsToFile();
                    FileMethods.DataRoot = FileMethods.FindSettingsElement("DataRoot");
                    _mainWindowViewModel.SaveMsgColor = Brushes.LimeGreen;
                    _mainWindowViewModel.SaveMsg = "Settings successfully saved";
                }
                catch (Exception)
                {
                    _mainWindowViewModel.SaveMsgColor = Brushes.DarkRed;
                    _mainWindowViewModel.SaveMsg = "An error occured while saving. Nothing changed.";
                }
            }
            else
            {
                if (_mainWindowViewModel.NewDataPath == null)
                {
                    try
                    {
                        FileMethods.SaveSettingsToFile();
                        _mainWindowViewModel.SaveMsgColor = Brushes.LimeGreen;
                        _mainWindowViewModel.SaveMsg = "Settings successfully saved";
                    }
                    catch (Exception)
                    {
                        _mainWindowViewModel.SaveMsgColor = Brushes.DarkRed;
                        _mainWindowViewModel.SaveMsg = "An error occured while saving. Nothing changed.";
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
                            FileMethods.DataRoot = FileMethods.FindSettingsElement("DataRoot");
                            _mainWindowViewModel.IsMigrate = false;
                            _mainWindowViewModel.SaveMsgColor = Brushes.LimeGreen;
                            _mainWindowViewModel.SaveMsg = "Settings successfully saved";
                        }
                        catch (Exception)
                        {
                            _mainWindowViewModel.SaveMsgColor = Brushes.DarkRed;
                            _mainWindowViewModel.SaveMsg = "An error occured while saving. Nothing changed.";
                        }
                    }
                }
            }
        }
    }
}
