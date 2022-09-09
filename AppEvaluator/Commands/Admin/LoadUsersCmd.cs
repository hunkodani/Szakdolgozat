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

        public override void Execute(object parameter)
        {
            _manageUsersViewModel.LoadUsers();
        }
    }
}
