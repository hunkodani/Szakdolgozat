using AppEvaluator.ViewModels.Admin;
using System;
using System.Windows;

namespace AppEvaluator.Commands.Admin
{
    internal class DeleteSubjectCmd : CommandBase
    {
        private readonly ManageSubjectsViewModel _manageSubjectsViewModel;

        public DeleteSubjectCmd(ManageSubjectsViewModel manageSubjectsViewModel)
        {
            _manageSubjectsViewModel = manageSubjectsViewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null)
            {
                /*_manageSubjectsViewModel.DelMessage = "No subject selected.";
                _manageSubjectsViewModel.DelMessageColor = Brushes.Red;*/
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
                    Logging.WriteToLog(LogTypes.Error, "Unable to delete subject, message:" + e.Message);
                    /*_manageSubjectsViewModel.UpDelMessage = "Subject deletion failed.";
                    _manageSubjectsViewModel.UpDelMessageColor = Brushes.Red;*/
                }
            }
        }
    }
}
