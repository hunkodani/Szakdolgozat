using AppEvaluator.Commands;
using AppEvaluator.Stores;
using AppEvaluator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AppEvaluator.ViewModels
{
    internal class AuthenticationViewModel : ViewModelBase
    {
        public ICommand AuthenticateUserCmd { get; }

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

        private string _errorMsg;

        public string ErrorMsg
        {
            get
            {
                return _errorMsg;
            }
            set
            {
                _errorMsg = value;
                OnPropertyChanged(nameof(ErrorMsg));
            }
        }

        private Visibility _errorMsgVis;

        public Visibility ErrorMsgVis
        {
            get 
            { 
                return _errorMsgVis; 
            }
            set 
            { 
                _errorMsgVis = value;
                OnPropertyChanged(nameof(ErrorMsgVis));
            }
        }

        public AuthenticationViewModel(NavigationStore navigationStore)
        {
            MainWindow.Instance?.HideLoginInformations();
            AuthenticateUserCmd = new AuthenticateUserCmd(this, navigationStore);
        }
    }
}
