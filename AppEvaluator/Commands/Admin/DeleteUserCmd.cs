using AppEvaluator.ViewModels;
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
            this._manageUsersViewModel = manageUsersViewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null)
            {
                _manageUsersViewModel.UpDelMessage = "No user selected.";
                _manageUsersViewModel.UpDelMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    //call delete method here
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
