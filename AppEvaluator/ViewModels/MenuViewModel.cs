using AppEvaluator.Commands;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using AppEvaluator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AppEvaluator.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService _navigationService;

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
            ToManageUsersCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateManageUsersViewModel));
            ToManageSubjectsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateManageSubjectsViewModel));
            ToManageTestsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateManageTestsViewModel));
            ToAddAssignmentsCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateAddAssingmentsViewModel));
            LogoutCmd = new NavigateCmd(navigationService);
            ExitCmd = new ExitCmd();
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
            return new AddAssignmentsViewModel(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_navigationStore, _navigationService);
        }
    }
}
