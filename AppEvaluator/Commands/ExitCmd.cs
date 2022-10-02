using AppEvaluator.Services;

namespace AppEvaluator.Commands
{
    internal class ExitCmd : CommandBase
    {
        /// <summary>
        /// Shuts down the application
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            App.Current.Shutdown();
        }
    }
}
