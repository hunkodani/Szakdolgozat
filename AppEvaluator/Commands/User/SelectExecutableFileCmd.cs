using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppEvaluator.Commands.User
{
    internal class SelectExecutableFileCmd : CommandBase
    {
        private readonly RunTestsViewModel _runTestsViewModel;

        public SelectExecutableFileCmd(RunTestsViewModel runTestsViewModel)
        {
            this._runTestsViewModel = runTestsViewModel;
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
