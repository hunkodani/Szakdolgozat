using AppEvaluator.Services;

namespace AppEvaluator.Commands
{
    internal class NavigateCmd : CommandBase
    {
        private readonly NavigationService _navigationService;

        public NavigateCmd(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
