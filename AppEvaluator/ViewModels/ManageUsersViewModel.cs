using AppEvaluator.Commands;
using AppEvaluator.Commands.Admin;
using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;

namespace AppEvaluator.ViewModels
{
    internal class ManageUsersViewModel : ViewModelBase
    {
        private ObservableCollection<UserViewModel> _users;

        public IEnumerable<UserViewModel> Users => _users;

        private readonly List<Role> _roles;

        public IEnumerable<Role> Roles => _roles;

        public ManageUsersViewModel(NavigationService navigationService)
        {
            AddCmd = new AddUserCmd(this);
            UpdateCmd = new UpdateUserCmd(this);
            DeleteCmd = new DeleteUserCmd(this);
            LoadUsersCmd = new LoadUsersCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            _users = new ObservableCollection<UserViewModel>();
            _roles = new List<Role>();
            LoadUsers();
            LoadRoles();
        }

        public ICommand AddCmd { get; }
        public ICommand UpdateCmd { get; }
        public ICommand DeleteCmd { get; }
        public ICommand LoadUsersCmd { get; }
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

        private string _upDelMessage;
        public string UpDelMessage
        {
            get
            {
                return _upDelMessage;
            }
            set
            {
                _upDelMessage = value;
                OnPropertyChanged(nameof(UpDelMessage));
            }
        }

        private Brush _upDelMessageColor;

        public Brush UpDelMessageColor
        {
            get
            {
                return _upDelMessageColor;
            }
            set
            {
                _upDelMessageColor = value;
                OnPropertyChanged(nameof(UpDelMessageColor));
            }
        }
        #endregion

        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _code;

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        private int _roleId;

        public int RoleId
        {
            get
            {
                return _roleId;
            }
            set
            {
                _roleId = value;
                OnPropertyChanged(nameof(RoleId));
            }
        }

        private UserViewModel _updatedUser;
        public UserViewModel UpdatedUser
        {
            get
            {
                return _updatedUser;
            }
            set
            {
                _updatedUser = value;
                OnPropertyChanged(nameof(UpdatedUser));
            }
        }

        public void CreateUpdatableUser(UserViewModel user)
        {
            _updatedUser = user;
            OnPropertyChanged(nameof(UpdatedUser));
        }

        internal void LoadUsers()
        {
            List<User> users = WcfDataParser.UsersParse(WcfService.MainProxy?.GetUsers());
            _users.Clear();
            if (users != null)
            {
                foreach (User user in users)
                {
                    _users.Add(new UserViewModel(user));
                }
            }
        }

        internal void LoadRoles()
        {
            List<Role> roles = WcfDataParser.RolesParse(WcfService.MainProxy?.GetRoles());
            _roles.Clear();
            if (roles != null)
            {
                foreach (Role role in roles)
                {
                    _roles.Add(role);
                }
            }
        }
    }
}
