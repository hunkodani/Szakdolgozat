using AppEvaluator.ViewModels.Teacher;
using System.Windows.Forms;

namespace AppEvaluator.Commands.Teacher
{
    internal class AddDescFileCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;

        public AddDescFileCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Txt/Word files |*.txt; *.doc; *.docx"
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _manageTestsViewModel.DescFile = new FileStructure("Desc_" + dialog.SafeFileName, dialog.FileName);
            }
        }
    }
}
