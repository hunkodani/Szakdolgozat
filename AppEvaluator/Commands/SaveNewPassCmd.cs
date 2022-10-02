using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.ViewModels;
using ServerContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppEvaluator.Commands
{
    internal class SaveNewPassCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;

        public SaveNewPassCmd(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        /// <summary>
        /// Saves the new password for the User in the database
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (_settingsViewModel.NewPass == string.Empty ||_settingsViewModel.NewPass == "" ||
                _settingsViewModel.NewPassAgain == string.Empty || _settingsViewModel.NewPassAgain == "" ||
                _settingsViewModel.NewPass == null || _settingsViewModel.NewPassAgain == null)
            {
                _settingsViewModel.PassErrorMsg = "Password field(s) is/are empty.";
                _settingsViewModel.PassErrorMsgColor = Brushes.Red;
                _settingsViewModel.PassErrorMsgVis = System.Windows.Visibility.Visible;
                return;
            }
            if (_settingsViewModel.NewPass != _settingsViewModel.NewPassAgain)
            {
                _settingsViewModel.PassErrorMsg = "Password fields not mathcing.";
                _settingsViewModel.PassErrorMsgColor = Brushes.Red;
                _settingsViewModel.PassErrorMsgVis = System.Windows.Visibility.Visible;
                return;
            }

            bool succes = WcfService.MainProxy.SaveNewPassword((int)Stores.LoginDataStore.UserLoginData.UserId,
                                                               EncrypterDecrypterService.Encrypt(_settingsViewModel.NewPass, EncrypterDecrypterService.Key));

            if (succes)
            {
                _settingsViewModel.PassErrorMsg = "New password saved.";
                _settingsViewModel.PassErrorMsgColor = Brushes.Green;
                _settingsViewModel.PassErrorMsgVis = System.Windows.Visibility.Visible;
            }
            else
            {
                _settingsViewModel.PassErrorMsg = "Error at saving, please try again.";
                _settingsViewModel.PassErrorMsgColor = Brushes.Red;
                _settingsViewModel.PassErrorMsgVis = System.Windows.Visibility.Visible;
            }
        }
    }
}
