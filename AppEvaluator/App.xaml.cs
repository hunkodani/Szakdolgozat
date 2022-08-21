using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Stores;
using AppEvaluator.ViewModels;
using AppEvaluator.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            NetworkMethods.GetServerAddress();

            _navigationStore.CurrentViewModel = new AuthenticationViewModel(_navigationStore);
            //_navigationStore.CurrentViewModel = new MenuViewModel(_navigationStore, null);
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
            NetworkingAndWCF.WcfService.MainCommunicationChannel?.Close();
            NetworkingAndWCF.WcfService.FileChannel?.Close();

            base.OnExit(e);
        }
    }
}
