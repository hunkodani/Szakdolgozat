using AppEvaluatorServer.FileManupulationAndSQL;
using AppEvaluatorServer.ViewModels;

namespace AppEvaluatorServer.Commands
{
    internal class ToggleMigrationCmd : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ToggleMigrationCmd(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        /// <summary>
        /// Updates the Migration setting in the settings list
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            int index = FileMethods.FindSettingsElementIndex("Migration");
            if (index != -1)
            {
                FileMethods.Settings[index] = new string[] { "Migration", _mainWindowViewModel.IsMigrate.ToString() };
            }
            else
            {
                FileMethods.Settings.Add(new string[] { "Migration", _mainWindowViewModel.IsMigrate.ToString() });
            }
        }
    }
}
