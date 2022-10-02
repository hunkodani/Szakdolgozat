using AppEvaluator.ViewModels.Teacher;

namespace AppEvaluator.Commands.Teacher
{
    internal class LoadTestsCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;
        private readonly AddAssignmentsViewModel _addAssignmentsViewModel;
        private readonly DeleteAssignmentsViewModel _deleteAssignmentsViewModel;

        public LoadTestsCmd(ManageTestsViewModel manageTestsViewModel)
        {
            _manageTestsViewModel = manageTestsViewModel;
        }

        public LoadTestsCmd(AddAssignmentsViewModel addAssignmentsViewModel)
        {
            _addAssignmentsViewModel = addAssignmentsViewModel;
        }

        public LoadTestsCmd(DeleteAssignmentsViewModel deleteAssignmentsViewModel)
        {
            _deleteAssignmentsViewModel = deleteAssignmentsViewModel;
        }

        /// <summary>
        /// Calls the LoadTests method across multiple viewmodel (depending which calls it)
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_manageTestsViewModel != null)
            {
                _manageTestsViewModel.LoadTests();
            }
            else if (_addAssignmentsViewModel != null)
            {
                _addAssignmentsViewModel.LoadTests();
            }
            else
            {
                _deleteAssignmentsViewModel.LoadTests();
            }
        }
    }
}
