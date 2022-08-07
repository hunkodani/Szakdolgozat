using AppEvaluator.Services;

namespace AppEvaluator.Commands
{
    internal class ExitCmd : CommandBase
    {
        public override void Execute(object parameter)
        {
            App.Current.Shutdown();
        }
    }
}
