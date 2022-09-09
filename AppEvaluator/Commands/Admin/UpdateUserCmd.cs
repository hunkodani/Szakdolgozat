using AppEvaluator.ViewModels.Admin;
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

        public override void Execute(object parameter)
        {
            if (_manageUsersViewModel.UpdatedUser.Password != null ||
                _manageUsersViewModel.UpdatedUser.Code != null)
            {
                _manageUsersViewModel.UpDelMessage = "Some fields are left empty, please fill them.";
                _manageUsersViewModel.UpDelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    //update user call here
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
