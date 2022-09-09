using AppEvaluator.ViewModels;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Properties;
using System.Net;
using System.Windows.Media;

namespace AppEvaluator.Commands
{
    internal class ApplySettingsCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;

        public ApplySettingsCmd(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        public override void Execute(object parameter)
        {
            Settings.Default.IsMulticast = _settingsViewModel.IsMulticastEna;
            Settings.Default.IsStaticIP = _settingsViewModel.IsStaticIPEna;
            Settings.Default.IPAddress = _settingsViewModel.IPAddress;
            if (_settingsViewModel.IsStaticIPEna && (_settingsViewModel.IPAddress == "" || _settingsViewModel.IPAddress == string.Empty))
            {
                Stores.ConnectionStore.ConnectionStatus = false;
                _settingsViewModel.ConnErrorMsg = "Missing IP address.";
                _settingsViewModel.ConnErrorMsgColor = Brushes.Red;
                _settingsViewModel.ConnErrorMsgVis = System.Windows.Visibility.Visible;
                return;
            }
            Settings.Default.Save();
            NetworkMethods.receiveThread?.Abort();
            NetworkMethods.McastSocket?.Dispose();
            WcfService.MainCommunicationChannel?.Close();
            WcfService.FileChannel?.Close();
            if (Settings.Default.IsStaticIP)
            {
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
