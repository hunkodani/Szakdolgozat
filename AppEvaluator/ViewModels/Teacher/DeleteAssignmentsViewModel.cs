using AppEvaluator.Commands;
using AppEvaluator.Commands.Teacher;
using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels.Teacher
{
    internal class DeleteAssignmentsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService _navigationService;
        private readonly ObservableCollection<UserAssignmentViewModel> _users;
        private readonly ObservableCollection<TestViewModel> _tests;
        private readonly List<SubjectViewModel> _cBSubjects;

        internal class UserAssignmentViewModel : UserViewModel
        {
            public bool Selected { get; set; }
            public UserAssignmentViewModel(User user) : base(user)
            {
            }
        }

        public IEnumerable<UserAssignmentViewModel> Users => _users;
        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<SubjectViewModel> CBSubjects => _cBSubjects;

        public ICommand DeleteAssignmentCmd { get; }
        public ICommand LoadTestsCmd { get; }
        public ICommand ToAddAssingmentsCmd { get; }
        public ICommand BackToMenuCmd { get; }

        private TestViewModel _selectedTest;

        public TestViewModel SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
                if (_selectedTest != null)
                {
                    LoadUsers();
                }
                else
                {
                    _users.Clear();
                    OnPropertyChanged(nameof(Users));
                }
            }
        }

        private SubjectViewModel _selectedSubject;

        public SubjectViewModel SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
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


        public DeleteAssignmentsViewModel(NavigationStore navigationStore, NavigationService navigationService)
        {
            _navigationStore = navigationStore;
            _navigationService = navigationService;
            DeleteAssignmentCmd = new DeleteAssignmentCmd(this);
            LoadTestsCmd = new LoadTestsCmd(this);
            ToAddAssingmentsCmd = new NavigateCmd(navigationService);
            BackToMenuCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateMenuViewModel));

            _users = new ObservableCollection<UserAssignmentViewModel>();
            _tests = new ObservableCollection<TestViewModel>();
            _cBSubjects = new List<SubjectViewModel>();

            LoadSubjects();
        }

        /// <summary>
        /// Loads the subjects from the database
        /// </summary>
        internal void LoadSubjects()
        {
            List<Subject> subjects = WcfDataParser.SubjectsParse(WcfService.MainProxy?.GetSubjects());
            _cBSubjects.Clear();
            if (subjects != null)
            {
                foreach (Subject subject in subjects)
                {
                    _cBSubjects.Add(new SubjectViewModel(subject));
                }
            }
        }

        /// <summary>
        /// Loads the selected subject's tests from the database
        /// </summary>
        internal void LoadTests()
        {
            List<Test> tests = null;
            if (SelectedSubject != null)
            {
                tests = WcfDataParser.TestsParse(WcfService.MainProxy?.GetTests(SelectedSubject.Code));
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

        /// <summary>
        /// Loads the Users associated with the selected test (have an assignment)
        /// </summary>
        internal void LoadUsers()
        {
            List<User> users = WcfDataParser.UsersParse(WcfService.MainProxy?.GetUsersOnTest(SelectedTest.TestId));
            _users.Clear();
            if (users != null)
            {
                foreach (User user in users)
                {
                    _users.Add(new UserAssignmentViewModel(user));
                }
            }
        }

        private MenuViewModel CreateMenuViewModel()
        {
            return new MenuViewModel(_navigationStore, new NavigationService(_navigationStore, CreateDeleteAssignmentsViewModel));
        }

        private DeleteAssignmentsViewModel CreateDeleteAssignmentsViewModel()
        {
            return new DeleteAssignmentsViewModel(_navigationStore, _navigationService);
        }
    }
}
