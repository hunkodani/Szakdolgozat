using AppEvaluator.Commands;
using AppEvaluator.Commands.User;
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

namespace AppEvaluator.ViewModels.Teacher
{
    internal class ViewUserTestResultsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TestViewModel> _tests;
        private readonly List<SubjectViewModel> _subjects;
        private readonly ObservableCollection<UserViewModel> _users;

        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<SubjectViewModel> Subjects => _subjects;
        public IEnumerable<UserViewModel> Users => _users;

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
                LoadUsers();
            }
        }

        private UserViewModel _selectedUser;

        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
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

        public ViewUserTestResultsViewModel(NavigationService navigationService)
        {
            ReadDescriptionCmd = new ReadDescriptionCmd(this);
            ReadResultCmd = new RunTestCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            _subjects = new List<SubjectViewModel>();
            _tests = new ObservableCollection<TestViewModel>();
            _users = new ObservableCollection<UserViewModel>();
            LoadSubjects();
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
            _tests.Clear();
            _users.Clear();
        }

        /// <summary>
        /// Loads the selected subject's tests from the database
        /// </summary>
        internal void LoadTests()
        {
            List<Test> tests = null;
            if (_selectedSubject != null)
            {
                tests = WcfDataParser.TestsParse(WcfService.MainProxy?.GetTests(_selectedSubject.Code));
            }
            _tests.Clear();
            if (tests != null)
            {
                foreach (Test test in tests)
                {
                    _tests.Add(new TestViewModel(test));
                }
            }
            _users.Clear();
        }

        /// <summary>
        /// Loads the Users associated with the selected test (have an assignment)
        /// </summary>
        internal void LoadUsers()
        {
            List<User> users = null;
            if (_selectedSubject != null && _selectedTest != null)
            {
                users = WcfDataParser.UsersParse(WcfService.MainProxy?.GetUsersOnTest(_selectedTest.TestId));
            }
            _users.Clear();
            if (users != null)
            {
                foreach (User user in users)
                {
                    _users.Add(new UserViewModel(user));
                }
            }
        }
    }
}
