using AppEvaluator.Stores;
using AppEvaluator.ViewModels;
using System;

namespace AppEvaluator.Services
{
    internal class NavigationService
    {
        private readonly NavigationStore _navigateStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public NavigationService(NavigationStore navigateStore, Func<ViewModelBase> createViewModel)
        {
            _navigateStore = navigateStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigateStore.CurrentViewModel = _createViewModel();
        }
    }
}
