using AppEvaluator.ViewModels.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Teacher
{
    internal class DeleteAssignmentCmd : CommandBase
    {
        private readonly DeleteAssignmentsViewModel _deleteAssignmentsViewModel;

        public DeleteAssignmentCmd(DeleteAssignmentsViewModel deleteAssignmentsViewModel)
        {
            _deleteAssignmentsViewModel = deleteAssignmentsViewModel;
        }

        /// <summary>
        /// Deletes a selected assignment from the database for all users that have been selected
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_deleteAssignmentsViewModel.SelectedTest == null ||
                _deleteAssignmentsViewModel.Users.Count() == 0)
            {
                _deleteAssignmentsViewModel.Message = "Selection incomplete, please select one test and one or multiple user(s).";
                _deleteAssignmentsViewModel.MessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    bool isUserSelected = false;
                    foreach (var user in _deleteAssignmentsViewModel.Users)
                    {
                        if (user.Selected)
                        {
                            NetworkingAndWCF.WcfService.MainProxy?.DeleteAssignment(
                                userId: user.UserId ?? default,
                                testId: _deleteAssignmentsViewModel.SelectedTest.TestId
                                );
                            isUserSelected = true;
                        }
                    }

                    if (isUserSelected)
                    {
                        _deleteAssignmentsViewModel.Message = "Assignment deletion requests sent.";
                        _deleteAssignmentsViewModel.MessageColor = Brushes.Green;
                        _deleteAssignmentsViewModel.LoadUsers();
                    }
                    else
                    {
                        _deleteAssignmentsViewModel.Message = "Assignment deletion aborted. No user selected.";
                        _deleteAssignmentsViewModel.MessageColor = Brushes.Red;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to delete assignments, message:" + e.Message);
                    _deleteAssignmentsViewModel.Message = "Assignment deletion failed.";
                    _deleteAssignmentsViewModel.MessageColor = Brushes.Red;
                }
            }
        }
    }
}
