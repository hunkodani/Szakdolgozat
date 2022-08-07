using AppEvaluator.Models;
using AppEvaluator.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
                ((dynamic)this.DataContext).CreateUpdatableUser((UserViewModel)UserListView.SelectedItem);
            }
        }

        private void PasswordUpdate(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).UpdatedUser.Password = ((PasswordBox)sender).Password;
            }
        }

        private void RoleSelected(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                Role role = (Role)e.AddedItems[0];
                //Role role = (Role)((ComboBox)sender).SelectedItem;
                ((dynamic)this.DataContext).RoleId = role.RoleId;
            }
        }
    }
}
