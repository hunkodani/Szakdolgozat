using AppEvaluatorServer.FileManupulationAndSQL;
using AppEvaluatorServer.ViewModels;
using System.IO;
using System.Windows.Forms;

namespace AppEvaluatorServer.Commands
{
    internal class PickRootFolderCmd : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public PickRootFolderCmd(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        /// <summary>
        /// Picks a root folder location, then updates the settings list with it
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _mainWindowViewModel.FolderPathLbl = Path.Combine(folderBrowserDialog.SelectedPath, FileMethods.DataDirectoryName);
                _mainWindowViewModel.NewDataPath = _mainWindowViewModel.FolderPathLbl;
                int index = FileMethods.FindSettingsElementIndex("DataRoot");
                if (index != -1)
                {
                    FileMethods.Settings[index] = new string[] { "DataRoot", _mainWindowViewModel.NewDataPath };
                }
                else
                {
                    FileMethods.Settings.Add(new string[] { "DataRoot", _mainWindowViewModel.NewDataPath });
                }
            }
        }
    }
}
