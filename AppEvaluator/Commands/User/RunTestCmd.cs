using AppEvaluator.ViewModels.Teacher;
using AppEvaluator.ViewModels.UserVMs;
using AppEvaluator.NetworkingAndWCF;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppEvaluator.Commands.User
{
    internal class RunTestCmd : CommandBase
    {
        private readonly RunTestsViewModel _runTestsViewModel;
        private readonly ViewTestResultsViewModel _viewTestResultsViewModel;
        private readonly ViewUserTestResultsViewModel _viewUserTestResultsViewModel;
        private readonly static string testPrefix = "Test";

        public RunTestCmd(RunTestsViewModel runTestsViewModel)
        {
            this._runTestsViewModel = runTestsViewModel;
        }

        public RunTestCmd(ViewTestResultsViewModel viewTestResultsViewModel)
        {
            this._viewTestResultsViewModel = viewTestResultsViewModel;
        }

        public RunTestCmd(ViewUserTestResultsViewModel viewUserTestResultsViewModel)
        {
            this._viewUserTestResultsViewModel = viewUserTestResultsViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_runTestsViewModel != null)
            {
                ExecuteAtRuntTests();
            }
            else if (_viewTestResultsViewModel != null)
            {
                ExecuteAtViewTestResults();
            }
            else if (_viewUserTestResultsViewModel != null)
            {
                ExecuteAtViewUserTestResults();
            }
        }

        private async void ExecuteAtRuntTests()
        {
            try
            {
                _runTestsViewModel.FileContent = "";
                //Getting the file paths and downloading them
                List<string> testFileNames = WcfService.FileProxy?.GetTestFileNames(_runTestsViewModel.SelectedTest.TestId);
                for (int i = 0; i < testFileNames.Count; i++)
                {
                    using (Stream stream = await WcfService.FileProxy.DownloadTestFile(testFileNames[i]))
                    {
                        if (stream != null && stream.CanRead)
                        {
                            using (FileStream fs = new FileStream(testPrefix + testFileNames[i] + "_" + _viewTestResultsViewModel.SelectedTest.TestName, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                await stream.CopyToAsync(fs);
                                fs.Close();
                            }
                        }
                    }
                }

                //Do stuff here


                //Delete the unnecessary files (all of the downloaded ones)(, when working properly)
                for (int i = 0; i < testFileNames.Count; i++)
                {
                    File.Delete(testPrefix + testFileNames[i] + "_" + _viewTestResultsViewModel.SelectedTest.TestName);
                }
            }
            catch (Exception e)
            {
                _runTestsViewModel.FileContent = "Error when evaluating: " + e.Message;
            }
            _runTestsViewModel.ContentType = "Test result:";
        }

        private void ExecuteAtViewTestResults()
        {
            try
            {
                _viewTestResultsViewModel.FileContent = "";
            }
            catch (Exception e)
            {
                _viewTestResultsViewModel.FileContent = "Error when evaluating: " + e.Message;
            }
            _viewTestResultsViewModel.ContentType = "Test result:";
        }

        private void ExecuteAtViewUserTestResults()
        {
            try
            {
                _viewUserTestResultsViewModel.FileContent = "";
            }
            catch (Exception e)
            {
                _viewUserTestResultsViewModel.FileContent = "Error when evaluating: " + e.Message;
            }
            _viewUserTestResultsViewModel.ContentType = "Test result:";
        }
    }
}
