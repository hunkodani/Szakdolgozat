using AppEvaluator.Commands;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppEvaluator.ViewModels.Teacher
{
    internal class DeleteAssignmentsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService _navigationService;

        public ICommand ToAddAssingmentsCmd { get; }
        public ICommand BackToMenuCmd { get; }

        public DeleteAssignmentsViewModel(NavigationStore navigationStore, NavigationService navigationService)
        {
            _navigationStore = navigationStore;
            _navigationService = navigationService;
            ToAddAssingmentsCmd = new NavigateCmd(navigationService);
            BackToMenuCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateMenuViewModel));
        }

        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_navigationStore, new NavigationService(_navigationStore, CreateDeleteAssignmentsViewModel));
        }

        private DeleteAssignmentsViewModel CreateDeleteAssignmentsViewModel()
        {
            return new DeleteAssignmentsViewModel(_navigationStore, _navigationService);
        }
    }
}
