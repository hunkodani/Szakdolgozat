using AppEvaluator.Commands;
using AppEvaluator.Commands.Admin;
using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels
{
    internal class ManageUsersViewModel : ViewModelBase
    {
        private readonly ObservableCollection<UserViewModel> _users;

        public IEnumerable<UserViewModel> Users => _users;

        public ManageUsersViewModel()
        {
            AddCommand = new AddUserCommand(this);
            UpdateCommand = new UpdateUserCommand();
            DeleteCommand = new DeleteUserCommand();
            BackToMenuCommand = new BackToMenuCommand();
            _users = new ObservableCollection<UserViewModel>();
            //temporary items
            _users.Add(new UserViewModel(new User("asd", "asd", "asd", 1, "asd", 1)));
            _users.Add(new UserViewModel(new User("adssd", "aasdsd", "adsd", 1, "assssd", 1)));
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand BackToMenuCommand { get; }

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

        public void createUpdatableUser(UserViewModel user)
        {
            _updatedUser = user;
            OnPropertyChanged(nameof(UpdatedUser));
        }

    }
}
