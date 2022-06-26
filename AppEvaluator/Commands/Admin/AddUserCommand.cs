using AppEvaluator.Models;
using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Admin
{
    internal class AddUserCommand : CommandBase
    {
        private readonly ManageUsersViewModel _manageUsersViewModel;

        public AddUserCommand(ManageUsersViewModel manageUsersViewModel)
        {
            this._manageUsersViewModel = manageUsersViewModel;
        }

        public override void Execute(object parameter)
        {
            if (!(_manageUsersViewModel.Username != null &&
                _manageUsersViewModel.Password != null &&
                _manageUsersViewModel.Code != null))
            {
                _manageUsersViewModel.AddMessage = "Not all fields are filled, please fill in everything.";
                _manageUsersViewModel.AddMessageColor = Brushes.Red;
            }
            else try
            {
                NetworkMethods.SendInsertUser(
                username: _manageUsersViewModel.Username,
                password: _manageUsersViewModel.Password,
                code: _manageUsersViewModel.Code,
                role: _manageUsersViewModel.RoleId + 1
                );
                _manageUsersViewModel.AddMessage = "User creation request sent.";
                _manageUsersViewModel.AddMessageColor = Brushes.Green;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                _manageUsersViewModel.AddMessage = "User creation failed.";
                _manageUsersViewModel.AddMessageColor = Brushes.Red;
            }
        }
    }
}
