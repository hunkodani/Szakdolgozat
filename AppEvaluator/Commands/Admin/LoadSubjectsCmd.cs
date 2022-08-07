using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Commands.Admin
{
    internal class LoadSubjectsCmd : CommandBase
    {
        private readonly ManageSubjectsViewModel _manageSubjectsViewModel;

        public LoadSubjectsCmd(ManageSubjectsViewModel manageSubjectsViewModel)
        {
            this._manageSubjectsViewModel = manageSubjectsViewModel;
        }

        public override void Execute(object parameter)
        {
            _manageSubjectsViewModel.LoadSubjects();
        }
    }
}
