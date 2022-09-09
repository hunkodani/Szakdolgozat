using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
    }
}
