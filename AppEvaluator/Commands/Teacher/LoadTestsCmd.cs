using AppEvaluator.ViewModels.Teacher;

namespace AppEvaluator.Commands.Teacher
{
    internal class LoadTestsCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;
        private readonly AddAssignmentsViewModel _addAssignmentsViewModel;

        public LoadTestsCmd(ManageTestsViewModel manageTestsViewModel)
        {
            _manageTestsViewModel = manageTestsViewModel;
        }

        public LoadTestsCmd(AddAssignmentsViewModel addAssignmentsViewModel)
        {
            _addAssignmentsViewModel = addAssignmentsViewModel;
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
