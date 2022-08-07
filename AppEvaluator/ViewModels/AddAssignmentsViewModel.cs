using AppEvaluator.Commands;
using AppEvaluator.Commands.Teacher;
using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels
{
    internal class AddAssignmentsViewModel : ViewModelBase
    {
        internal class UserAssignmentViewModel : UserViewModel
        {
            public bool Selected { get; set; }
            public UserAssignmentViewModel(User user) : base(user)
            {
            }
        }

        private readonly ObservableCollection<UserAssignmentViewModel> _users;
        private readonly ObservableCollection<TestViewModel> _tests;

        public IEnumerable<UserAssignmentViewModel> Users => _users;

        public ICollectionView Tests
        {
            get
            {
                var source = CollectionViewSource.GetDefaultView(_tests);
                source.GroupDescriptions.Add(new PropertyGroupDescription("SubjectCode"));
                return source;
            }
        }

        public ICommand CreateAssignmentCmd { get; }
        public ICommand ToDeleteAssignmentCmd { get; }
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
        #endregion

        public AddAssignmentsViewModel(NavigationService navigationService)
        {
            CreateAssignmentCmd = new CreateAssignmentCmd(this);
            ToDeleteAssignmentCmd = new ToDeleteAssignmentCmd();
            BackToMenuCmd = new NavigateCmd(navigationService);
            _users = new ObservableCollection<UserAssignmentViewModel>();
            _tests = new ObservableCollection<TestViewModel>();

            LoadTests();
            LoadUsers();
        }

        internal void LoadTests()
        {
            List<Test> tests = WcfDataParser.TestsParse(WcfService.MainProxy?.GetTests("Info-123"));
            _tests.Clear();
            if (tests != null)
            {
                foreach (Test test in tests)
                {
                    _tests.Add(new TestViewModel(test));
                }
            }
        }

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
    }
}
