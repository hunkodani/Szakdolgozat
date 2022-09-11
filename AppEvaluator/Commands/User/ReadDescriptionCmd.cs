using AppEvaluator.ViewModels.Teacher;
using AppEvaluator.ViewModels.UserVMs;
using System;
using AppEvaluator.NetworkingAndWCF;
using System.IO;
using System.Windows.Media;

namespace AppEvaluator.Commands.User
{
    internal class ReadDescriptionCmd : CommandBase
    {
        private readonly RunTestsViewModel _runTestsViewModel;
        private readonly ViewUserTestResultsViewModel _viewUserTestResultsViewModel;
        private readonly ViewTestResultsViewModel _viewTestResultsViewModel;

        public ReadDescriptionCmd(RunTestsViewModel runTestsViewModel)
        {
            this._runTestsViewModel = runTestsViewModel;
        }

        public ReadDescriptionCmd(ViewUserTestResultsViewModel viewUserTestResultsViewModel)
        {
            this._viewUserTestResultsViewModel = viewUserTestResultsViewModel;
        }

        public ReadDescriptionCmd(ViewTestResultsViewModel viewTestResultsViewModel)
        {
            this._viewTestResultsViewModel = viewTestResultsViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_runTestsViewModel != null)
            {
                ExecuteRunTestsViewModel();
            }
            else if (_viewUserTestResultsViewModel != null)
            {
                ExecuteViewUserTestResultsViewModel();
            }
            else if (_viewTestResultsViewModel != null)
            {
                ExecuteViewTestResultsViewModel();
            }
        }

        private async void ExecuteRunTestsViewModel()
        {
            if (_runTestsViewModel.SelectedTest == null)
            {
                _runTestsViewModel.Message = "No selected test, please select one after selecting a subject.";
                _runTestsViewModel.MessageColor = Brushes.Red;
                return;
            }
            try
            {
                _runTestsViewModel.FileContent = "";
                using (Stream stream = await WcfService.FileProxy.DownloadDescription(_runTestsViewModel.SelectedTest.TestId))
                {
                    if (stream != null && stream.CanRead)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            _runTestsViewModel.FileContent = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _runTestsViewModel.FileContent = "Error loading description: " + e.Message;
            }
            _runTestsViewModel.Message = "";
            _runTestsViewModel.ContentType = "Description:";
        }

        private async void ExecuteViewUserTestResultsViewModel()
        {
            if (_viewUserTestResultsViewModel.SelectedTest == null)
            {
                _viewUserTestResultsViewModel.Message = "No selected test, please select one after selecting a subject.";
                _viewUserTestResultsViewModel.MessageColor = Brushes.Red;
                return;
            }
            try
            {
                _viewUserTestResultsViewModel.FileContent = "";
                using (Stream stream = await WcfService.FileProxy.DownloadDescription(_viewUserTestResultsViewModel.SelectedTest.TestId))
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
                _viewUserTestResultsViewModel.FileContent = "Error loading description: " + e.Message;
            }
            _viewUserTestResultsViewModel.Message = "";
            _viewUserTestResultsViewModel.ContentType = "Description:";
        }

        private async void ExecuteViewTestResultsViewModel()
        {
            if (_viewTestResultsViewModel.SelectedTest == null)
            {
                _viewTestResultsViewModel.Message = "No selected test, please select one after selecting a subject.";
                _viewTestResultsViewModel.MessageColor = Brushes.Red;
                return;
            }
            try
            {
                _viewTestResultsViewModel.FileContent = "";
                using (Stream stream = await WcfService.FileProxy.DownloadDescription(_viewTestResultsViewModel.SelectedTest.TestId))
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
                _viewTestResultsViewModel.FileContent = "Error loading description: " + e.Message;
            }
            _viewTestResultsViewModel.Message = "";
            _viewTestResultsViewModel.ContentType = "Description:";
        }
    }
}
