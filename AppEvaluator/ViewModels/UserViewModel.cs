using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.ViewModels
{
    internal class UserViewModel : ViewModelBase
    {
        private readonly User _user;

        public int? UserId => _user.UserId;
        public string Username => _user.Username;
        public string Password
        {
            get
            {
                return _user.Password;
            }
            set
            {
                _user.Password = value;
            }
        }

        public string Code
        {
            get
            {
                return _user.Code;
            }
            set
            {
                _user.Code = value;
            }
        }

        public int RoleId => _user.RoleId;
        public string FolderLocation => _user.FolderLocation;

        public UserViewModel(User user)
        {
            _user = user;
        }
    }
}
