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

namespace AppEvaluator.Views
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {

        internal static string AccessLevel { get; set; }
        public Menu()
        {
            InitializeComponent();
        }

        private void MenuStartup(object sender, RoutedEventArgs e)
        {
            string tag;
            foreach (Border child in FindVisualChilds<Border>(this))
            {
                if (child.Tag == null)
                {
                    child.Visibility = Visibility.Visible;
                    continue;
                }

                tag = child.Tag.ToString();
                switch (AccessLevel.ToLower())
                {
                    case "user":
                        if (tag == "User")
                        {
                            child.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            child.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case "teacher":
                        if (tag != "Admin")
                        {
                            child.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            child.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case "admin":
                        child.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }

        public static IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChilds<T>(ithChild)) yield return childOfChild;
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Teacher_AddTest(Main);
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
    }
}
