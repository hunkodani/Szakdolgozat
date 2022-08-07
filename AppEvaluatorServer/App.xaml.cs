﻿using AppEvaluatorServer.WcfServices;
using ServerContracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Windows;

namespace AppEvaluatorServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            /*MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();*/

            ISelectionService mainCommunicationService = new MainCommunicationService();
            Uri[] uris = new Uri[1]
            {
                new Uri($"net.tcp://{NetworkMethods.LocalIPAddress}:{NetworkMethods.ServerPort}/MainService")
            };
            NetworkMethods.SelectionHost = new ServiceHost(mainCommunicationService, uris);
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            NetworkMethods.SelectionHost.AddServiceEndpoint(typeof(ISelectionService), binding, "");
            NetworkMethods.SelectionHost.Opened += Host_Opened;
            NetworkMethods.SelectionHost.Open();

            IFileService fileService = new FileService();
            uris = new Uri[1]
            {
                new Uri($"net.tcp://{NetworkMethods.LocalIPAddress}:{NetworkMethods.ServerPort}/FileService")
            };
            NetworkMethods.FileHost = new ServiceHost(fileService, uris);
            binding.TransferMode = TransferMode.Streamed;
            NetworkMethods.FileHost.AddServiceEndpoint(typeof(IFileService), binding, "");
            NetworkMethods.FileHost.Opened += Host_Opened;
            NetworkMethods.FileHost.Open();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NetworkMethods.SelectionHost.Closed += Host_Closed;
            NetworkMethods.FileHost.Closed += Host_Closed;
            NetworkMethods.SelectionHost.Close();
            NetworkMethods.FileHost.Close();

            base.OnExit(e);
        }

        private void Host_Closed(object sender, EventArgs e)
        {
            
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            
        }
    }
}
