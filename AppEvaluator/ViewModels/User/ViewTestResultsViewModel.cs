using AppEvaluator.Commands;
using AppEvaluator.Commands.User;
using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels.UserVMs
{
    internal class ViewTestResultsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TestViewModel> _tests;
        private readonly List<SubjectViewModel> _subjects;

        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<SubjectViewModel> Subjects => _subjects;

        public ICommand ReadDescriptionCmd { get; }
        public ICommand ReadResultCmd { get; }
        public ICommand BackToMenuCmd { get; }

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
                LoadTests();
            }
        }

        private TestViewModel _selectedTest;

        public TestViewModel SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        private string _contentType;

        public string ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                OnPropertyChanged(nameof(ContentType));
            }
        }

        private string _fileContent;

        public string FileContent
        {
            get { return _fileContent; }
            set
            {
                _fileContent = value;
                OnPropertyChanged(nameof(FileContent));
            }
        }

        #region Messages And Brushes

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private Brush _messageColor;

        public Brush MessageColor
        {
            get
            {
                return _messageColor;
            }
            set
            {
                _messageColor = value;
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        #endregion

        public ViewTestResultsViewModel(NavigationService navigationService)
        {
            ReadDescriptionCmd = new ReadDescriptionCmd(this);
            ReadResultCmd = new RunTestCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            _subjects = new List<SubjectViewModel>();
            _tests = new ObservableCollection<TestViewModel>();
            LoadSubjects();
        }

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

        internal void LoadTests()
        {
            List<Test> tests = null;
            if (_selectedSubject != null)
            {
                tests = WcfDataParser.TestsParse(WcfService.MainProxy?.GetUserAvailableTest(_selectedSubject.Code, (int)LoginDataStore.UserLoginData.UserId));
            }
            _tests.Clear();
            if (tests != null)
            {
                foreach (Test test in tests)
                {
                    _tests.Add(new TestViewModel(test));
                }
            }
        }
    }
}
