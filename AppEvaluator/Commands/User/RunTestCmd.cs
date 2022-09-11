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
            Stream uploadStream = null;
            //string evaluationFileName = "";
            try
            {
                _runTestsViewModel.FileContent = "";
                //Getting the file paths and downloading them
                List<string> testFilePaths = WcfService.FileProxy?.GetTestFileNames(_runTestsViewModel.SelectedTest.TestId);
                for (int i = 0; i < testFilePaths.Count; i++)
                {
                    using (Stream stream = await WcfService.FileProxy.DownloadTestFile(testFilePaths[i]))
                    {
                        if (stream != null && stream.CanRead)
                        {
                            using (FileStream fs = new FileStream(testPrefix + i + "_" + _runTestsViewModel.SelectedTest.TestName, 
                                                                  FileMode.OpenOrCreate, 
                                                                  FileAccess.Write))
                            {
                                await stream.CopyToAsync(fs);
                                fs.Close();
                            }
                        }
                    }
                }

                //Do evaluation and make a file from it



                //Delete the unnecessary files (all of the downloaded ones)(, when working properly)
                for (int i = 0; i < testFilePaths.Count; i++)
                {
                    File.Delete(testPrefix + i + "_" + _runTestsViewModel.SelectedTest.TestName);
                }

                //Upload evaluation, show here in FileContent, then delete it (not working, while evaluation is not generating file)
                /*using (uploadStream = File.OpenRead(evaluationFileName))
                {
                    await WcfService.FileProxy.UploadEvaluationFile(
                        new ServerContracts.Models.FileUpload(fileName: evaluationFileName,
                                                              fileStreamer: uploadStream,
                                                              toRelativeLocation: Stores.LoginDataStore.UserLoginData.FolderLocation));
                }
                _runTestsViewModel.FileContent = File.ReadAllText(evaluationFileName);
                File.Delete(evaluationFileName);*/
            }
            catch (Exception e)
            {
                _runTestsViewModel.FileContent = "Error when evaluating: " + e.Message;
                uploadStream?.Close();
            }
            _runTestsViewModel.ContentType = "Test result:";
        }

        private async void ExecuteAtViewTestResults()
        {
            try
            {
                _viewTestResultsViewModel.FileContent = "";
                string path = Path.Combine(Stores.LoginDataStore.UserLoginData.FolderLocation, _viewTestResultsViewModel.SelectedTest.TestName);
                using (Stream stream = await WcfService.FileProxy?.DownloadEvaluationFile(path))
                {
                    if (stream != null && stream.CanRead)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            _viewTestResultsViewModel.FileContent = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _viewTestResultsViewModel.FileContent = "Error at evaluation download: " + e.Message;
            }
            _viewTestResultsViewModel.ContentType = "Test result:";
        }

        private async void ExecuteAtViewUserTestResults()
        {
            try
            {
                _viewUserTestResultsViewModel.FileContent = "";
                string path = Path.Combine(Stores.LoginDataStore.UserLoginData.FolderLocation, _viewUserTestResultsViewModel.SelectedTest.TestName);
                using (Stream stream = await WcfService.FileProxy?.DownloadEvaluationFile(path))
                {
                    if (stream != null && stream.CanRead)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            _viewUserTestResultsViewModel.FileContent = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _viewUserTestResultsViewModel.FileContent = "Error at evaluation download: " + e.Message;
            }
            _viewUserTestResultsViewModel.ContentType = "Test result:";
        }
    }
}
