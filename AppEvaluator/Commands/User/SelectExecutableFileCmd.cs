using AppEvaluator.ViewModels.Teacher;
using AppEvaluator.ViewModels.UserVMs;
using System.Windows.Forms;

namespace AppEvaluator.Commands.User
{
    internal class SelectExecutableFileCmd : CommandBase
    {
        private readonly RunTestsViewModel _runTestsViewModel;

        public SelectExecutableFileCmd(RunTestsViewModel runTestsViewModel)
        {
            _runTestsViewModel = runTestsViewModel;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Exe Files (.exe)|*.exe"
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _runTestsViewModel.SelectedFile = new FileStructure(dialog.SafeFileName, dialog.FileName);
            }
        }
    }
}
