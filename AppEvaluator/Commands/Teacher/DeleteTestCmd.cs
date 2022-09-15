using AppEvaluator.ViewModels.Teacher;
using System;
using System.IO;
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
            if (_manageTestsViewModel.SelectedTest == null)
            {
                _manageTestsViewModel.UpDelMessage = "No test selected.";
                _manageTestsViewModel.UpDelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    NetworkingAndWCF.WcfService.MainProxy?.DeleteTest(_manageTestsViewModel.SelectedTest.TestId,
                                                                      Path.Combine(_manageTestsViewModel.SelectedTest.SubjectCode,
                                                                                   _manageTestsViewModel.SelectedTest.TestName));
                    _manageTestsViewModel.UpDelMessage = "Test deletion message sent.";
                    _manageTestsViewModel.UpDelMessageColor = Brushes.Green;
                    _manageTestsViewModel.LoadTests();
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
