using AppEvaluator.ViewModels.Teacher;
using System.Windows.Forms;

namespace AppEvaluator.Commands.Teacher
{
    internal class AddTestFileCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;

        public AddTestFileCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Txt files (*.txt)|*.txt"
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _manageTestsViewModel.TestFiles.Add(new FileStructure(dialog.SafeFileName, dialog.FileName));
            }
        }
    }
}
