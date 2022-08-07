using System;
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
using AppEvaluator.Views.Admin;
using AppEvaluator.ViewModels;
using AppEvaluator.Views.Teacher;

namespace AppEvaluator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public void ShowLoginInformations(string username, string role)
        {
            LoginLbl.Visibility = Visibility.Visible;
            LoginAccountLbl.Content = username + " as " + role;
            LoginAccountLbl.Visibility = Visibility.Visible;
        }

        public void HideLoginInformations()
        {
            LoginLbl.Visibility = Visibility.Hidden;
            LoginAccountLbl.Visibility = Visibility.Hidden;
        }
    }
}
