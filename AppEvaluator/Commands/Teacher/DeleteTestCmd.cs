using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Teacher
{
    internal class DeleteTestCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;

        public DeleteTestCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null)
            {
                _manageTestsViewModel.UpDelMessage = "No test selected.";
                _manageTestsViewModel.UpDelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    //call delete method here
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to delete test, message:" + e.Message);
                    _manageTestsViewModel.UpDelMessage = "Test deletion failed.";
                    _manageTestsViewModel.UpDelMessageColor = Brushes.Red;
                }
            }
        }
    }
}
