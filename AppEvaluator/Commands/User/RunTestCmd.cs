using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Commands.User
{
    internal class RunTestCmd : CommandBase
    {
        private readonly RunTestsViewModel _runTestsViewModel;

        public RunTestCmd(RunTestsViewModel runTestsViewModel)
        {
            this._runTestsViewModel = runTestsViewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                _runTestsViewModel.FileContent = "";
            }
            catch (Exception e)
            {
                _runTestsViewModel.FileContent = "Error when evaluating: " + e.Message;
            }
            _runTestsViewModel.ContentType = "Test result:";
        }
    }
}
