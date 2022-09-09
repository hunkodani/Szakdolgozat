using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Stores;
using AppEvaluator.ViewModels;
using AppEvaluator.Views;
using System.Net;
using System.Windows;

namespace AppEvaluator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (AppEvaluator.Properties.Settings.Default.IsMulticast)
            {
                NetworkMethods.GetServerAddress();
            }
            else if (AppEvaluator.Properties.Settings.Default.IsStaticIP && 
                     (AppEvaluator.Properties.Settings.Default.IPAddress != "" &&
                     AppEvaluator.Properties.Settings.Default.IPAddress != string.Empty))
            {
                WcfService.ConnectToServices(IPAddress.Parse(AppEvaluator.Properties.Settings.Default.IPAddress), 
                                                             AppEvaluator.Properties.Settings.Default.ClientPort - 1);
            }

            _navigationStore.CurrentViewModel = new AuthenticationViewModel(_navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ///Aborts the Multicast listening thread by closing the sokcet
            if (NetworkMethods.McastSocket != null)
            {
                NetworkMethods.McastSocket.Close();
            }
            WcfService.MainCommunicationChannel?.Close();
            WcfService.FileChannel?.Close();

            base.OnExit(e);
        }
    }
}
