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

        /// <summary>
        /// Downloads the evaluation testcases, calls the evaluation, show the result in the page, then uploads the result. Deletes every file that created
        /// </summary>
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

                ///Creates the evaluation element that will evaluate
                FileEvaluation fileEvaluation = new FileEvaluation(_runTestsViewModel.SelectedFile.Location,
                                                                   testFilePaths.Count,
                                                                   _runTestsViewModel.SelectedTest.TestName);
                 await fileEvaluation.Execute();


                ///Deletes the unnecessary files (all of the downloaded ones)
                for (int i = 0; i < testFilePaths.Count; i++)
                {
                    File.Delete(testPrefix + i + "_" + _runTestsViewModel.SelectedTest.TestName);
                }

                ///Uploads the evaluation, and also shows here in FileContent, then deletes it
                using (uploadStream = File.OpenRead(fileEvaluation.ResultPath))
                {
                    await WcfService.FileProxy.UploadEvaluationFile(
                        new ServerContracts.Models.FileUpload(fileName: "Result_" + _runTestsViewModel.SelectedTest.TestName,
                                                              fileStreamer: uploadStream,
                                                              toRelativeLocation: Stores.LoginDataStore.UserLoginData.FolderLocation));
                }
                _runTestsViewModel.FileContent = File.ReadAllText(fileEvaluation.ResultPath);
                File.Delete(fileEvaluation.ResultPath);
            }
            catch (Exception e)
            {
                _runTestsViewModel.FileContent = "Error when evaluating: " + e.Message;
                uploadStream?.Close();
            }
            _runTestsViewModel.ContentType = "Test result:";
        }

        /// <summary>
        /// Donwloads the selected test result and load it to the viewport
        /// </summary>
        private async void ExecuteAtViewTestResults()
        {
            try
            {
                _viewTestResultsViewModel.FileContent = "";
                string path = Path.Combine(Stores.LoginDataStore.UserLoginData.FolderLocation, "Result_" + _viewTestResultsViewModel.SelectedTest.TestName);
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
            catch (System.ServiceModel.CommunicationException)
            {
                _viewTestResultsViewModel.FileContent = "No file can be retrieved or timeout has been reached.";
            }
            catch (Exception e)
            {
                _viewTestResultsViewModel.FileContent = "Error at evaluation download: " + e.Message;
            }
            _viewTestResultsViewModel.ContentType = "Test result:";
        }

        /// <summary>
        /// Donwloads the selected test result and load it to the viewport
        /// </summary>
        private async void ExecuteAtViewUserTestResults()
        {
            try
            {
                _viewUserTestResultsViewModel.FileContent = "";
                string path = Path.Combine(_viewUserTestResultsViewModel.SelectedUser.FolderLocation, "Result_" + _viewUserTestResultsViewModel.SelectedTest.TestName);
                Stream stream = await WcfService.FileProxy?.DownloadEvaluationFile(path);
                if (stream != null)
                {
                    using (stream)
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
            }
            catch (System.ServiceModel.CommunicationException)
            {
                _viewUserTestResultsViewModel.FileContent = "No file can be retrieved or timeout has been reached.";
            }
            catch (Exception e)
            {
                _viewUserTestResultsViewModel.FileContent = "Error at evaluation download: " + e.Message;
            }
            _viewUserTestResultsViewModel.ContentType = "Test result:";
        }
    }
}
