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
    /// Interaction logic for Student_RunTest.xaml
    /// </summary>
    public partial class Student_RunTest : Page
    {
        public string DirPath { get; private set; } = "";
        public Frame Main { get; }

        public Student_RunTest(Frame main)
        {
            InitializeComponent();
            Main = main;
            TestTypeLabel.Visibility = Visibility.Hidden;
            DirView.ItemsSource = DriveInfo.GetDrives();
        }

        /// <summary>
        /// Lists the directory and file names to which the path points to
        /// </summary>
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
                DirLabel.Visibility = Visibility.Hidden;
                DirView.IsEnabled = false;
                DirBackBtn.IsEnabled = false;
                SelectFileBtn.IsEnabled = false;
                TestTypeLabel.Visibility = Visibility.Visible;
                TestTypeView.IsEnabled = true;
                BackToSelectFileBtn.IsEnabled = true;
                RunTestBtn.IsEnabled = true;
            }
        }

        private void BackToSelectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            TestTypeLabel.Visibility = Visibility.Hidden;
            TestTypeView.IsEnabled = false;
            BackToSelectFileBtn.IsEnabled = false;
            RunTestBtn.IsEnabled = false;
            DirLabel.Visibility = Visibility.Visible;
            DirView.IsEnabled = true;
            DirBackBtn.IsEnabled = true;
            SelectFileBtn.IsEnabled = true;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.GoBack();
        }

        private void RunTestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TestTypeView.SelectedItem != null)
            {
                FileEvaluation fileEvaluation = new FileEvaluation(DirPath, DirView.SelectedItem.ToString());
                fileEvaluation.Evaluate();
            }

            /*
            FileEvaluation evaluation = new FileEvaluation(DirPath, DirView.SelectedItem.ToString());
            TestView.Items.Clear();
            try
            {
                try
                {
                    evaluation.Process.Start();
                    while (true)  //itt nem lehet endofstream, mert akkor ha elfogy az olvasnivaló akkor leállna, függetlenül, hogy van-e még utána más
                    {
                        if (evaluation.Process.HasExited) //&& evaluation.Process.ExitCode > -1)   //Cannot put HasExited because it returns true if there is an input required (-xxxxxxxxx negative value)
                        {
                            while (!evaluation.Process.StandardOutput.EndOfStream)
                            {
                                TestView.Items.Add(evaluation.Process.StandardOutput.ReadLine());
                            }
                            break;
                        }
                        else
                        {   //nagyon belassítja ez az ellenőrzés a folyamatot... -> más megoldás kellene: pl mikor üres sort olvas be, akkor ellenőrizni ezt.
                            if (!(evaluation.Process.StandardOutput.Peek() == -1))// && evaluation.Process.Threads[0].ThreadState == System.Diagnostics.ThreadState.Wait &&
                            //evaluation.Process.Threads[0].WaitReason == System.Diagnostics.ThreadWaitReason.UserRequest))
                            {
                                TestView.Items.Add(evaluation.Process.StandardOutput.ReadLine());
                            }
                            else
                            {
                                //billentyűlenyomást kellene átküldeni.... vagy: nem lehet a kódban readkey (a végén lehet, mert a rendszer leállítja, mert úgy érzékeli, mintha végzett volna)!!!
                                evaluation.Process.StandardInput.WriteLine();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    

                }
                finally
                {
                    if (!evaluation.Process.HasExited)
                    {
                        evaluation.Process.Close();
                    }
                    if (!evaluation.Process.HasExited)
                    {
                        evaluation.Process.Kill();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                evaluation.Process.Dispose();
            }
            */
        }
    }
}
