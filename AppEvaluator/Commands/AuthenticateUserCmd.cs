using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using AppEvaluator.ViewModels;
using AppEvaluator.Views;
using ServerContracts;
using System.Windows.Input;

namespace AppEvaluator.Commands
{
    internal class AuthenticateUserCmd : CommandBase
    {
        private readonly AuthenticationViewModel _authenticationViewModel;
        private readonly NavigationStore _navigationStore;

        public AuthenticateUserCmd(AuthenticationViewModel authenticationViewModel, Stores.NavigationStore navigationStore)
        {
            _authenticationViewModel = authenticationViewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {

            if (_authenticationViewModel.Username == null ||
                _authenticationViewModel.Username == string.Empty ||
                _authenticationViewModel.Password == null ||
                _authenticationViewModel.Password == string.Empty)
            {
                _authenticationViewModel.ErrorMsg = "Missing username or password!";
                _authenticationViewModel.ErrorMsgVis = System.Windows.Visibility.Visible;
            }
            else
            {
                _authenticationViewModel.ErrorMsgVis = System.Windows.Visibility.Collapsed;
                bool success = WcfDataParser.LoginDataParser(WcfService.MainProxy?.Login(_authenticationViewModel.Username, 
                                                                                         EncrypterDecrypterService.Encrypt(_authenticationViewModel.Password, EncrypterDecrypterService.Key)));
                if (success)
                {
                    LoginDataStore.RoleName = WcfService.MainProxy.GetRoleName(LoginDataStore.UserLoginData.RoleId);
                    MainWindow.Instance.ShowLoginInformations(LoginDataStore.UserLoginData.Username, LoginDataStore.RoleName);
                    ICommand navigate = new NavigateCmd(new NavigationService(_navigationStore, CreateMenuViewModel));
                    navigate.Execute(null);
                }
                else
                {
                    _authenticationViewModel.ErrorMsg = "Incorrect username or password!";
                    _authenticationViewModel.ErrorMsgVis = System.Windows.Visibility.Visible;
                }
            }
        }

        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_navigationStore, new NavigationService(_navigationStore, CreateAuthenticationViewModel));
        }

        private AuthenticationViewModel CreateAuthenticationViewModel()
        {
            return new AuthenticationViewModel(_navigationStore);
        }
    }
}
