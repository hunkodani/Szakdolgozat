using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.ViewModels.Teacher;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Teacher
{
    internal class CreateAssignmentCmd : CommandBase
    {
        private readonly AddAssignmentsViewModel _addAssignmentssViewModel;

        public CreateAssignmentCmd(AddAssignmentsViewModel addAssignmentssViewModel)
        {
            this._addAssignmentssViewModel = addAssignmentssViewModel;
        }

        /// <summary>
        /// Creates assignments in the database for every User that have been selected
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_addAssignmentssViewModel.SelectedTest == null ||
                _addAssignmentssViewModel.Users.Count() == 0)
            {
                _addAssignmentssViewModel.AddMessage = "Selection incomplete, please select one test and one or multiple user(s).";
                _addAssignmentssViewModel.AddMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    bool isUserSelected = false;
                    foreach (var user in _addAssignmentssViewModel.Users)
                    {
                        if (user.Selected)
                        {
                            WcfService.MainProxy?.InsertAssignment(
                                userId: user.UserId ?? default,
                                testId: _addAssignmentssViewModel.SelectedTest.TestId
                                );
                            isUserSelected = true;
                        }
                    }
                    
                    if (isUserSelected)
                    {
                        _addAssignmentssViewModel.AddMessage = "Assignment creation requests sent.";
                        _addAssignmentssViewModel.AddMessageColor = Brushes.Green;
                    }
                    else
                    {
                        _addAssignmentssViewModel.AddMessage = "Assignment creation aborted. No user selected.";
                        _addAssignmentssViewModel.AddMessageColor = Brushes.Red;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to insert assignments, message:" + e.Message);
                    _addAssignmentssViewModel.AddMessage = "Assignment creation failed.";
                    _addAssignmentssViewModel.AddMessageColor = Brushes.Red;
                }
            }
        }
    }
}
