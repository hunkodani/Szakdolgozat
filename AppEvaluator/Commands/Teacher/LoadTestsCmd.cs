using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Commands.Teacher
{
    internal class LoadTestsCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;

        public LoadTestsCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        public override void Execute(object parameter)
        {
            _manageTestsViewModel.LoadTests();
        }
    }
}
