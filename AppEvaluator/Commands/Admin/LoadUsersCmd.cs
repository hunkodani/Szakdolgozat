using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Commands.Admin
{
    internal class LoadUsersCmd : CommandBase
    {
        private readonly ManageUsersViewModel _manageUsersViewModel;

        public LoadUsersCmd(ManageUsersViewModel manageUsersViewModel)
        {
            this._manageUsersViewModel = manageUsersViewModel;
        }

        public override void Execute(object parameter)
        {
            _manageUsersViewModel.LoadUsers();
        }
    }
}
