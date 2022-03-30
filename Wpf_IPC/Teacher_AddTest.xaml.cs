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

namespace Wpf_IPC
{
    /// <summary>
    /// Interaction logic for Teacher_AddTest.xaml
    /// </summary>
    public partial class Teacher_AddTest : Page
    {
        public string DirPath { get; private set; } = "";
        public Frame Main { get; }

        public Teacher_AddTest(Frame main)
        {
            InitializeComponent();
            Main = main;
            DirView.ItemsSource = DriveInfo.GetDrives();
        }

        /// <summary>
        /// Lists the directory and file names to which the path points to
        /// </summary>S
        private void UpdateDirView()
        {
            try
            {
                List<string> tmp = new List<string>();
                Directory.GetFileSystemEntries(DirPath).ToList().ForEach(x => tmp.Add(x.Remove(0, DirPath.Length)));
                DirView.ItemsSource = tmp;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Creates a new directory at the current path with the given name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DirNameTB.Text != "" && !Regex.IsMatch(DirNameTB.Text, "[?:<>*/|\"]"))
            {
                if (!Directory.Exists(DirPath + DirNameTB.Text))
                {
                    Directory.CreateDirectory(DirPath + DirNameTB.Text);
                    UpdateDirView();
                }
            }
            else
            {
                //to do: popupwindow: invalid directory name
            }
        }

        /// <summary>
        /// Moves to the directory if it is exists, and lists the directory and file names in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DirView.SelectedItem != null && Directory.Exists(DirPath + DirView.SelectedItem.ToString() + @"\"))
            {
                DirPath += DirView.SelectedItem.ToString() + @"\";
                UpdateDirView();
            }
        }

        /// <summary>
        /// Returns to the parent directory and displays the items in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DirPath.Length > 4)
            {
                string tmp = DirPath.Remove(DirPath.Length - 1, 1);
                DirPath = tmp.Remove(tmp.LastIndexOf("\\") + 1);
                UpdateDirView();
            }
            else
            {
                DirPath = "";
                DirView.ItemsSource = DriveInfo.GetDrives();
            }
        }

        private void SelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DirView.SelectedItem != null && File.Exists(DirPath + DirView.SelectedItem.ToString()))
            {
                DirView.IsEnabled = false;
                DirBackBtn.IsEnabled = false;
                DirCreateBtn.IsEnabled = false;
                DirNameTB.IsEnabled = false;
                SelectFileBtn.IsEnabled = false;
                TestTypeView.IsEnabled = true;
                BackToSelectFileBtn.IsEnabled = true;
                RunTestBtn.IsEnabled = true;
            }
        }

        private void BackToSelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            TestTypeView.IsEnabled = false;
            BackToSelectFileBtn.IsEnabled = false;
            RunTestBtn.IsEnabled = false;
            DirView.IsEnabled = true;
            DirBackBtn.IsEnabled = true;
            DirCreateBtn.IsEnabled = true;
            DirNameTB.IsEnabled = true;
            SelectFileBtn.IsEnabled = true;
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.GoBack();
        }

        private void RunTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
