using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Commands.Teacher
{
    internal class LoadTestsCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;
        private readonly AddAssignmentsViewModel _addAssignmentsViewModel;

        public LoadTestsCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        public LoadTestsCmd(AddAssignmentsViewModel addAssignmentsViewModel)
        {
            this._addAssignmentsViewModel = addAssignmentsViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_manageTestsViewModel != null)
            {
                _manageTestsViewModel.LoadTests();
            }
            else
            {
                _addAssignmentsViewModel.LoadTests();
            }
        }
    }
}
