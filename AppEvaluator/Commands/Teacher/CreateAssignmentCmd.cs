﻿using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
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
    internal class CreateAssignmentCmd : CommandBase
    {
        private readonly AddAssignmentsViewModel _addAssignmentssViewModel;

        public CreateAssignmentCmd(AddAssignmentsViewModel addAssignmentssViewModel)
        {
            this._addAssignmentssViewModel = addAssignmentssViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_addAssignmentssViewModel.Tests.CurrentItem == null ||
                _addAssignmentssViewModel.Users.Count() == 0)
            {
                _addAssignmentssViewModel.AddMessage = "Selection incomplete, please select one test and one or multiple user(s).";
                _addAssignmentssViewModel.AddMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    foreach (var user in _addAssignmentssViewModel.Users)
                    {
                        if (user.Selected)
                        {
                            NetworkMethods.SendInsertAssignment(
                                userId: user.UserId ?? default,
                                testId: ((TestViewModel)_addAssignmentssViewModel.Tests.CurrentItem).TestId
                                );
                        }
                    }
                    
                    _addAssignmentssViewModel.AddMessage = "Assignment creation requests sent.";
                    _addAssignmentssViewModel.AddMessageColor = Brushes.Green;
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
