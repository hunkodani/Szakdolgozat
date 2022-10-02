using AppEvaluator.ViewModels.Admin;
using System;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Admin
{
    internal class DeleteUserCmd : CommandBase
    {
        private readonly ManageUsersViewModel _manageUsersViewModel;

        public DeleteUserCmd(ManageUsersViewModel manageUsersViewModel)
        {
            _manageUsersViewModel = manageUsersViewModel;
        }

        /// <summary>
        /// Removes a User from the database
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_manageUsersViewModel.SelectedUser == null)
            {
                _manageUsersViewModel.UpDelMessage = "No user selected.";
                _manageUsersViewModel.UpDelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    NetworkingAndWCF.WcfService.MainProxy?.DeleteUser(_manageUsersViewModel.SelectedUser.UserId ?? default,
                                                                      _manageUsersViewModel.SelectedUser.FolderLocation);
                    _manageUsersViewModel.UpDelMessage = "User deletion message sent.";
                    _manageUsersViewModel.UpDelMessageColor = Brushes.Green;
                    _manageUsersViewModel.LoadUsers();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to delete user, message:" + e.Message);
                    _manageUsersViewModel.UpDelMessage = "User deletion failed.";
                    _manageUsersViewModel.UpDelMessageColor = Brushes.Red;
                }
            }
            
        }
    }
}
