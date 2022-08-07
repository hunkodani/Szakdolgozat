using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AppEvaluator.Commands.Admin
{
    internal class AddSubjectCmd : CommandBase
    {
        private readonly ManageSubjectsViewModel _manageSubjectsViewModel;

        public AddSubjectCmd(ManageSubjectsViewModel manageSubjectsViewModel)
        {
            this._manageSubjectsViewModel = manageSubjectsViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_manageSubjectsViewModel.SubjectCode == null ||
                _manageSubjectsViewModel.SubjectName == null ||
                _manageSubjectsViewModel.SubjectCode == String.Empty ||
                _manageSubjectsViewModel.SubjectName == String.Empty)
            {
                _manageSubjectsViewModel.AddMessage = "Not all fields are filled, please fill in everything.";
                _manageSubjectsViewModel.AddMessageColor = Brushes.Red;
            }
            else
            {
                try
                {
                    NetworkMethods.SendInsertSubject(
                    subjectCode: _manageSubjectsViewModel.SubjectCode,
                    subjectName: _manageSubjectsViewModel.SubjectName
                    );
                    _manageSubjectsViewModel.AddMessage = "Subject creation request sent.";
                    _manageSubjectsViewModel.AddMessageColor = Brushes.Green;
                    _manageSubjectsViewModel.LoadSubjects();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                    Logging.WriteToLog(LogTypes.Error, "Unable to insert subject, message:" + e.Message);
                    _manageSubjectsViewModel.AddMessage = "Subject creation failed.";
                    _manageSubjectsViewModel.AddMessageColor = Brushes.Red;
                }
            }
        }
    }
}
