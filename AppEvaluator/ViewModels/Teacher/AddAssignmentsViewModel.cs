﻿using AppEvaluator.Commands;
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
    internal class AddAssignmentsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService _navigationService;
        internal class UserAssignmentViewModel : UserViewModel
        {
            public bool Selected { get; set; }
            public UserAssignmentViewModel(User user) : base(user)
            {
            }
        }

        private readonly ObservableCollection<UserAssignmentViewModel> _users;
        private readonly ObservableCollection<TestViewModel> _tests;
        private readonly List<SubjectViewModel> _cBSubjects;

        public IEnumerable<UserAssignmentViewModel> Users => _users;
        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<SubjectViewModel> CBSubjects => _cBSubjects;

        public ICommand CreateAssignmentCmd { get; }
        public ICommand LoadTestsCmd { get; }
        public ICommand ToDeleteAssignmentCmd { get; }
        public ICommand BackToMenuCmd { get; }

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
        #endregion

        public AddAssignmentsViewModel(NavigationStore navigationStore, NavigationService navigationService)
        {
            _navigationStore = navigationStore;
            _navigationService = navigationService;
            CreateAssignmentCmd = new CreateAssignmentCmd(this);
            LoadTestsCmd = new LoadTestsCmd(this);
            ToDeleteAssignmentCmd = new NavigateCmd(new NavigationService(_navigationStore, CreateDeleteAssignmentsViewModel));
            BackToMenuCmd = new NavigateCmd(navigationService);
            _users = new ObservableCollection<UserAssignmentViewModel>();
            _tests = new ObservableCollection<TestViewModel>();
            _cBSubjects = new List<SubjectViewModel>();

            LoadSubjects();
            LoadUsers();
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
        /// Loads all the Users from the database
        /// </summary>
        internal void LoadUsers()
        {
            List<User> users = WcfDataParser.UsersParse(WcfService.MainProxy?.GetUsers());
            _users.Clear();
            if (users != null)
            {
                foreach (User user in users)
                {
                    _users.Add(new UserAssignmentViewModel(user));
                }
            }
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

        private DeleteAssignmentsViewModel CreateDeleteAssignmentsViewModel()
        {
            return new DeleteAssignmentsViewModel(_navigationStore, new NavigationService(_navigationStore, CreateAddAssignmentsViewModel));
        }

        private AddAssignmentsViewModel CreateAddAssignmentsViewModel()
        {
            return new AddAssignmentsViewModel(_navigationStore, _navigationService);
        }
    }
}
