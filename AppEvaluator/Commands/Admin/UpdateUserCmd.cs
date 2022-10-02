using AppEvaluator.ViewModels.Admin;
using ServerContracts;
using System;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Admin
{
    internal class UpdateUserCmd : CommandBase
    {
        private readonly ManageUsersViewModel _manageUsersViewModel;

        public UpdateUserCmd(ManageUsersViewModel manageUsersViewModel)
        {
            this._manageUsersViewModel = manageUsersViewModel;
        }

        /// <summary>
        /// Updates the User with the new informations (code and/or password)
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_manageUsersViewModel.UpdatedUser.Password == null &&
                _manageUsersViewModel.UpdatedUser.Code == null)
            {
                _manageUsersViewModel.UpDelMessage = "Both updatable fields are empty, nothing to change.";
                _manageUsersViewModel.UpDelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    //update user call here
                    if (_manageUsersViewModel.UpdatedUser.Password == null)
                    {
                        NetworkingAndWCF.WcfService.MainProxy.UpdateUser(_manageUsersViewModel.UpdatedUser.UserId ?? default,
                                                                         code: _manageUsersViewModel.UpdatedUser.Code);
                    }
                    else if (_manageUsersViewModel.UpdatedUser.Code == null)
                    {
                        NetworkingAndWCF.WcfService.MainProxy.UpdateUser(_manageUsersViewModel.UpdatedUser.UserId ?? default,
                                                                         pass: EncrypterDecrypterService.Encrypt(_manageUsersViewModel.UpdatedUser.Password,
                                                                                                                 EncrypterDecrypterService.Key));
                    }
                    else
                    {
                        NetworkingAndWCF.WcfService.MainProxy.UpdateUser(_manageUsersViewModel.UpdatedUser.UserId ?? default,
                                                                         code: _manageUsersViewModel.UpdatedUser.Code,
                                                                         pass: EncrypterDecrypterService.Encrypt(_manageUsersViewModel.UpdatedUser.Password,
                                                                                                                 EncrypterDecrypterService.Key));
                    }
                    _manageUsersViewModel.UpDelMessage = "User modification message sent.";
                    _manageUsersViewModel.UpDelMessageColor = Brushes.Green;
                    _manageUsersViewModel.LoadUsers();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to update user, message:" + e.Message);
                    _manageUsersViewModel.UpDelMessage = "User modification failed.";
                    _manageUsersViewModel.UpDelMessageColor = Brushes.Red;
                }
            }
        }
    }
}
