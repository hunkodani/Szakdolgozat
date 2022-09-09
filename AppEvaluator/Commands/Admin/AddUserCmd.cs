using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.ViewModels.Admin;
using ServerContracts;
using System;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Admin
{
    internal class AddUserCmd : CommandBase
    {
        private readonly ManageUsersViewModel _manageUsersViewModel;

        public AddUserCmd(ManageUsersViewModel manageUsersViewModel)
        {
            this._manageUsersViewModel = manageUsersViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_manageUsersViewModel.Username == null ||
                _manageUsersViewModel.Password == null ||
                _manageUsersViewModel.Code == null ||
                _manageUsersViewModel.Username == String.Empty ||
                _manageUsersViewModel.Password == String.Empty ||
                _manageUsersViewModel.Code == String.Empty)
            {
                _manageUsersViewModel.AddMessage = "Not all fields are filled, please fill in everything.";
                _manageUsersViewModel.AddMessageColor = Brushes.Red;
            }
            else
            {
                string passw = EncrypterDecrypterService.Encrypt(_manageUsersViewModel.Password, EncrypterDecrypterService.Key);
                try
                {
                    NetworkMethods.SendInsertUser(
                    username: _manageUsersViewModel.Username,
                    password: passw,
                    code: _manageUsersViewModel.Code,
                    role: _manageUsersViewModel.RoleId
                    );
                    _manageUsersViewModel.AddMessage = "User creation request sent.";
                    _manageUsersViewModel.AddMessageColor = Brushes.Green;
                    _manageUsersViewModel.LoadUsers();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to insert user, message:" + e.Message);
                    _manageUsersViewModel.AddMessage = "User creation failed.";
                    _manageUsersViewModel.AddMessageColor = Brushes.Red;
                }
            }
        }
    }
}
