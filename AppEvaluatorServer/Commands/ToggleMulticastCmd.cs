using AppEvaluatorServer.FileManupulationAndSQL;
using AppEvaluatorServer.ViewModels;

namespace AppEvaluatorServer.Commands
{
    internal class ToggleMulticastCmd : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ToggleMulticastCmd(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override void Execute(object parameter)
        {
            int index = FileMethods.FindSettingsElementIndex("Multicasting");
            if (index != -1)
            {
                FileMethods.Settings[index] = new string[] { "Multicasting", _mainWindowViewModel.MulticastStatus.ToString() };
            }
            else
            {
                FileMethods.Settings.Add(new string[] { "Multicasting", _mainWindowViewModel.MulticastStatus.ToString() });
            }
        }
    }
}
