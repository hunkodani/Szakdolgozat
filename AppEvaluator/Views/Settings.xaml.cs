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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void NewPassChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).NewPass = ((PasswordBox)sender).Password;
            }
        }

        private void NewPassAgainChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).NewPassAgain = ((PasswordBox)sender).Password;
            }
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null) return;

            if (Stores.LoginDataStore.UserLoginData != null)
            {
                ((dynamic)this.DataContext).IsAuthOK = Visibility.Visible;
            }
            else
            {
                ((dynamic)this.DataContext).IsAuthOK = Visibility.Collapsed;
            }
        }
    }
}
