using AppEvaluator.ViewModels.Admin;
using System;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Admin
{
    internal class DeleteSubjectCmd : CommandBase
    {
        private readonly ManageSubjectsViewModel _manageSubjectsViewModel;

        public DeleteSubjectCmd(ManageSubjectsViewModel manageSubjectsViewModel)
        {
            _manageSubjectsViewModel = manageSubjectsViewModel;
        }

        /// <summary>
        /// Removes a Subject from the database (and everything that connects to it)
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_manageSubjectsViewModel.SelectedSubject == null)
            {
                _manageSubjectsViewModel.DelMessage = "No subject selected.";
                _manageSubjectsViewModel.DelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    NetworkingAndWCF.WcfService.MainProxy?.DeleteSubject(_manageSubjectsViewModel.SelectedSubject.Code,
                                                                         _manageSubjectsViewModel.SelectedSubject.FolderLocation);
                    _manageSubjectsViewModel.DelMessage = "Subject deletion message sent.";
                    _manageSubjectsViewModel.DelMessageColor = Brushes.Green;
                    _manageSubjectsViewModel.LoadSubjects();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to delete subject, message:" + e.Message);
                    _manageSubjectsViewModel.DelMessage = "Subject deletion failed.";
                    _manageSubjectsViewModel.DelMessageColor = Brushes.Red;
                }
            }
        }
    }
}
