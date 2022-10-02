using AppEvaluator.ViewModels;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Properties;
using System.Net;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace AppEvaluator.Commands
{
    internal class ApplySettingsCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;
        private static readonly Regex _iPRegex = new Regex(@"^((25[0-5]|(2[0-4]|1[0-9]|[1-9]|)[0-9])(\.(?!$)|$)){4}$");

        public ApplySettingsCmd(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        /// <summary>
        /// Saves the connection settings and applies it to the running application
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (!_iPRegex.IsMatch(_settingsViewModel.IPAddress))
            {
                _settingsViewModel.ConnErrorMsg = "Invalid IP address. Changes discarded.";
                _settingsViewModel.ConnErrorMsgColor = Brushes.Red;
                _settingsViewModel.ConnErrorMsgVis = System.Windows.Visibility.Visible;
                return;
            }
            Settings.Default.IsMulticast = _settingsViewModel.IsMulticastEna;
            Settings.Default.IsStaticIP = _settingsViewModel.IsStaticIPEna;
            Settings.Default.IPAddress = _settingsViewModel.IPAddress;
            Settings.Default.Save();
            NetworkMethods.receiveThread?.Abort();
            NetworkMethods.McastSocket?.Dispose();
            WcfService.MainCommunicationChannel?.Close();
            WcfService.FileChannel?.Close();
            if (Settings.Default.IsStaticIP)
            {
                NetworkMethods.ServerIPAddress = IPAddress.Parse(Settings.Default.IPAddress);
                WcfService.ConnectToServices(IPAddress.Parse(Settings.Default.IPAddress), Settings.Default.ClientPort - 1);
            }
            else if (Settings.Default.IsMulticast)
            {
                NetworkMethods.GetServerAddress();
            }
            if (Stores.ConnectionStore.ConnectionStatus == null)
            {
                _settingsViewModel.IsServerConnected = false;
            }
            else
            {
                _settingsViewModel.IsServerConnected = (bool)Stores.ConnectionStore.ConnectionStatus;
            }
            _settingsViewModel.ConnErrorMsg = "Connection type saved.";
            _settingsViewModel.ConnErrorMsgColor = Brushes.Green;
            _settingsViewModel.ConnErrorMsgVis = System.Windows.Visibility.Visible;
        }
    }
}
