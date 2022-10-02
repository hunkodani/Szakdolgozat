using AppEvaluator.ViewModels.Admin;

namespace AppEvaluator.Commands.Admin
{
    internal class LoadUsersCmd : CommandBase
    {
        private readonly ManageUsersViewModel _manageUsersViewModel;

        public LoadUsersCmd(ManageUsersViewModel manageUsersViewModel)
        {
            _manageUsersViewModel = manageUsersViewModel;
        }

        /// <summary>
        /// Calls the LoadUsers method on the _manageUsersViewModel
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            _manageUsersViewModel.LoadUsers();
        }
    }
}
