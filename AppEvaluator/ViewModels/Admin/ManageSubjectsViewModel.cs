using AppEvaluator.Commands;
using AppEvaluator.Commands.Admin;
using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels.Admin
{
    internal class ManageSubjectsViewModel: ViewModelBase
    {
        private readonly ObservableCollection<SubjectViewModel> _subjects;

        public IEnumerable<SubjectViewModel> Subjects => _subjects;

        public ManageSubjectsViewModel(NavigationService navigationService)
        {
            AddSubjectCmd = new AddSubjectCmd(this);
            DeleteSubjectCmd = new DeleteSubjectCmd(this);
            LoadSubjectsCmd = new LoadSubjectsCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            _subjects = new ObservableCollection<SubjectViewModel>();

            LoadSubjects();
        }

        public ICommand AddSubjectCmd { get; }
        public ICommand DeleteSubjectCmd { get; }
        public ICommand LoadSubjectsCmd { get; }
        public ICommand BackToMenuCmd { get; }

        #region Messages And Brushes

        private string _addMessage;
        public string AddMessage
        {
            get
            {
                return _addMessage;
            }
            set
            {
                _addMessage = value;
                OnPropertyChanged(nameof(AddMessage));
            }
        }

        private Brush _addMessageColor;

        public Brush AddMessageColor
        {
            get
            {
                return _addMessageColor;
            }
            set
            {
                _addMessageColor = value;
                OnPropertyChanged(nameof(AddMessageColor));
            }
        }

        private string _delMessage;
        public string DelMessage
        {
            get
            {
                return _delMessage;
            }
            set
            {
                _delMessage = value;
                OnPropertyChanged(nameof(DelMessage));
            }
        }

        private Brush _delMessageColor;

        public Brush DelMessageColor
        {
            get
            {
                return _delMessageColor;
            }
            set
            {
                _delMessageColor = value;
                OnPropertyChanged(nameof(DelMessageColor));
            }
        }
        #endregion

        private string _subjectCode;

        public string SubjectCode
        {
            get 
            { 
                return _subjectCode; 
            }
            set 
            { 
                _subjectCode = value;
                OnPropertyChanged(nameof(SubjectCode));
            }
        }

        private string _subjectName;

        public string SubjectName
        {
            get 
            { 
                return _subjectName;
            }
            set 
            { 
                _subjectName = value;
                OnPropertyChanged(nameof(SubjectName));
            }
        }

        private SubjectViewModel _selectedSubject;
        public SubjectViewModel SelectedSubject
        {
            get
            {
                return _selectedSubject;
            }
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
            }
        }

        /// <summary>
        /// Loads the subjects from the database
        /// </summary>
        internal void LoadSubjects()
        {
            List<Subject> subjects = WcfDataParser.SubjectsParse(WcfService.MainProxy?.GetSubjects());
            _subjects.Clear();
            if (subjects != null)
            {
                foreach (Subject subject in subjects)
                {
                    _subjects.Add(new SubjectViewModel(subject));
                }
            }
        }
    }
}
