using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Teacher
{
    internal class AddTestCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;

        public AddTestCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        public async override void Execute(object parameter)
        {
            if (_manageTestsViewModel.TestName == null ||
                _manageTestsViewModel.SubjectCode == null ||
                _manageTestsViewModel.TestName == String.Empty ||
                _manageTestsViewModel.SubjectCode == String.Empty)
            {
                _manageTestsViewModel.AddMessage = "Not all fields are filled, please fill in everything.";
                _manageTestsViewModel.AddMessageColor = Brushes.Red;
            }
            else
            {
                Stream stream = null;
                try
                {
                    NetworkMethods.SendInsertTest(
                    testName: _manageTestsViewModel.TestName,
                    subjectCode: _manageTestsViewModel.SubjectCode
                    );
                    if (_manageTestsViewModel.DescFile != null)
                    {
                        using (stream = File.OpenRead(_manageTestsViewModel.DescFile.Location))
                        {
                            await NetworkingAndWCF.WcfService.FileProxy.SaveTestFilesToServerByName(new ServerContracts.Models.TestFileUpload(
                                _manageTestsViewModel.TestName,
                                _manageTestsViewModel.DescFile.Name,
                                stream));
                        }
                        _manageTestsViewModel.DescFile = null;
                    }
                    foreach (var item in _manageTestsViewModel.TestFiles)
                    {
                        using (stream = File.OpenRead(item.Location))
                        {
                            await NetworkingAndWCF.WcfService.FileProxy.SaveTestFilesToServerByName(new ServerContracts.Models.TestFileUpload(
                                _manageTestsViewModel.TestName,
                                item.Name,
                                stream));
                        }
                        _manageTestsViewModel.TestFiles.Remove(item);
                    }
                    _manageTestsViewModel.AddMessage = "Test and data creation request sent.";
                    _manageTestsViewModel.AddMessageColor = Brushes.Green;
                    _manageTestsViewModel.TestName = null;
                    _manageTestsViewModel.LoadTests();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to (fully)insert test, message:" + e.Message);
                    _manageTestsViewModel.AddMessage = "Test or data creation failed.";
                    _manageTestsViewModel.AddMessageColor = Brushes.Red;
                    stream?.Close();
                }
            }
        }
    }
}
