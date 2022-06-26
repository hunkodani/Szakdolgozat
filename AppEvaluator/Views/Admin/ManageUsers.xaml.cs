using AppEvaluator.ViewModels;
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

namespace AppEvaluator.Views.Admin
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsers : UserControl
    {
        public ManageUsers()
        {
            InitializeComponent();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { 
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; 
            }
        }

        private void ShowModifySection(object sender, RoutedEventArgs e)
        {
            if (ModifySection.IsVisible)
            {
                ModifySection.Visibility = Visibility.Collapsed;
            }
            else if (UserListView.SelectedItem != null)
            {
                ModifySection.Visibility = Visibility.Visible;
            }
        }

        private void UpdateModifySection(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).createUpdatableUser((UserViewModel)UserListView.SelectedItem);
            }
        }

        private void PasswordUpdate(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).UpdatedUser.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
