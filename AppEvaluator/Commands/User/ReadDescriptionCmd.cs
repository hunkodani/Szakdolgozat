using AppEvaluator.ViewModels;
using AppEvaluator.ViewModels.TeacherVMs;
using AppEvaluator.ViewModels.UserVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                try
                {
                    _runTestsViewModel.FileContent = "";
                }
                catch (Exception e)
                {
                    _runTestsViewModel.FileContent = "Error loading description: " + e.Message;
                }
                _runTestsViewModel.ContentType = "Description:";
            }
            else if (_viewUserTestResultsViewModel != null)
            {
                try
                {
                    _viewUserTestResultsViewModel.FileContent = "";
                }
                catch (Exception e)
                {
                    _viewUserTestResultsViewModel.FileContent = "Error loading description: " + e.Message;
                }
                _viewUserTestResultsViewModel.ContentType = "Description:";
            }
            else if (_viewTestResultsViewModel != null)
            {
                try
                {
                    _viewTestResultsViewModel.FileContent = "";
                }
                catch (Exception e)
                {
                    _viewTestResultsViewModel.FileContent = "Error loading description: " + e.Message;
                }
                _viewTestResultsViewModel.ContentType = "Description:";
            }
        }
    }
}
