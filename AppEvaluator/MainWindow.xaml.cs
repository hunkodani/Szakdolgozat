﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppEvaluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NetworkMethods.GetServerAddress();
            Main.Content = new Authentication(this, Main);
        }

        public void ShowLoginInformations(string username, string role)
        {
            LoginLbl.Visibility = Visibility.Visible;
            LoginAccountLbl.Content = username + " / " + role;
            LoginAccountLbl.Visibility = Visibility.Visible;
        }

        public void HideLoginInformations()
        {
            LoginLbl.Visibility = Visibility.Hidden;
            LoginAccountLbl.Visibility = Visibility.Hidden;
        }

        private void DisposeElements(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ///Aborts the Multicast listening thread by closing the sokcet
            if (NetworkMethods.McastSocket != null)
            {
                NetworkMethods.McastSocket.Close();
            }
        }
    }
}