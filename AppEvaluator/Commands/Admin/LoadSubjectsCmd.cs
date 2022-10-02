using AppEvaluator.ViewModels.Admin;

namespace AppEvaluator.Commands.Admin
{
    internal class LoadSubjectsCmd : CommandBase
    {
        private readonly ManageSubjectsViewModel _manageSubjectsViewModel;

        public LoadSubjectsCmd(ManageSubjectsViewModel manageSubjectsViewModel)
        {
            _manageSubjectsViewModel = manageSubjectsViewModel;
        }

        /// <summary>
        /// Calls the LoadSubjects method on the _manageSubjectsViewModel
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            _manageSubjectsViewModel.LoadSubjects();
        }
    }
}
