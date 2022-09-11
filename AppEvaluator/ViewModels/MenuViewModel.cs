using AppEvaluator.Commands;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using AppEvaluator.ViewModels.Admin;
using AppEvaluator.ViewModels.Teacher;
using AppEvaluator.ViewModels.UserVMs;
using AppEvaluator.Views;
using System.Windows.Input;

namespace AppEvaluator.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService _navigationService;

        public ICommand ToRunTestsCmd { get; }
        public ICommand ToViewTestResultsCmd { get; }
        public ICommand ToViewUserTestResultsCmd { get; }
        public ICommand ToManageUsersCmd { get; }
        public ICommand ToManageSubjectsCmd { get; }
        public ICommand ToManageTestsCmd { get; }
        public ICommand ToAddAssignmentsCmd { get; }
        public ICommand ToSettingsCmd { get; }
        public ICommand LogoutCmd { get; }
        public ICommand ExitCmd { get; }

        public MenuViewModel(NavigationStore navigationStore, NavigationService navigationService)
        {
            _navigationStore = navigationStore;
            _navigationService = navigationService;
            Menu.AccessLevel = LoginDataStore.RoleName;
            ToRunTestsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateRunTestsViewModel));
            ToViewTestResultsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateViewTestResultsViewModel));
            ToViewUserTestResultsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateViewUserTestResultsViewModel));
            ToManageUsersCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateManageUsersViewModel));
            ToManageSubjectsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateManageSubjectsViewModel));
            ToManageTestsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateManageTestsViewModel));
            ToAddAssignmentsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateAddAssingmentsViewModel));
            ToSettingsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateSettingsVIewModel));
            LogoutCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateAuthenticationVIewModel));
            ExitCmd = new ExitCmd();
        }

        private RunTestsViewModel CreateRunTestsViewModel()
        {
            return new RunTestsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private ViewTestResultsViewModel CreateViewTestResultsViewModel()
        {
            return new ViewTestResultsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private ViewUserTestResultsViewModel CreateViewUserTestResultsViewModel()
        {
            return new ViewUserTestResultsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private ManageUsersViewModel CreateManageUsersViewModel()
        {
            return new ManageUsersViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private ManageSubjectsViewModel CreateManageSubjectsViewModel()
        {
            return new ManageSubjectsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private ManageTestsViewModel CreateManageTestsViewModel()
        {
            return new ManageTestsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private AddAssignmentsViewModel CreateAddAssingmentsViewModel()
        {
            return new AddAssignmentsViewModel(_navigationStore, new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private SettingsViewModel CreateSettingsVIewModel()
        {
            return new SettingsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private AuthenticationViewModel CreateAuthenticationVIewModel()
        {
            return new AuthenticationViewModel(_navigationStore);
        }

        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_navigationStore, _navigationService);
        }
    }
}
