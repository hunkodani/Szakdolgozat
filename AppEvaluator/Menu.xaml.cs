using System;
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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Frame Main { get; }

        public Menu(Frame main)
        {
            Main = main;
            InitializeComponent();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            /**before shutdown all network connections needs to be terminated (no movement, no connection)*/
            Application.Current.Shutdown();
        }

        #region Teacher

        private void ViewTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            //NetworkMethods.SendTcpMessage("unkown", "send testfiles", "TestSuccess");
        }

        #endregion

        #region Student

        private void RunTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        #endregion

        #region AdminSpecific

        private void ManageUsers(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void ManageTests(object sender, RoutedEventArgs e)
        {
            Main.Content = new Teacher_AddTest(Main);
        }

        private void ManageSubjects(object sender, RoutedEventArgs e)
        {

        }

        private void ManageAssignments(object sender, RoutedEventArgs e)
        {

        }

    }
}
