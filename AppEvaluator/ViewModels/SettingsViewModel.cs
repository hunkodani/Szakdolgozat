using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AppEvaluator.Commands;
using AppEvaluator.Services;

namespace AppEvaluator.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        public ICommand SaveServerConnCmd { get; }
        public ICommand SaveNewPassCmd { get; }
        public ICommand BackToMenuCmd { get; }

        private bool _isMulticastEna;

        public bool IsMulticastEna
        {
            get { return _isMulticastEna; }
            set 
            { 
                _isMulticastEna = value;
                OnPropertyChanged(nameof(IsMulticastEna));
            }
        }

        private bool _isStaticIPEna;

        public bool IsStaticIPEna
        {
            get { return _isStaticIPEna; }
            set
            {
                _isStaticIPEna = value;
                OnPropertyChanged(nameof(IsStaticIPEna));
            }
        }

        private string _iPAddress;

        public string IPAddress
        {
            get { return _iPAddress; }
            set
            {
                _iPAddress = value;
                OnPropertyChanged(nameof(IPAddress));
            }
        }

        private bool _isServerConnected;
        
        public bool IsServerConnected
        {
            get { return _isServerConnected; }
            set
            {
                _isServerConnected = value;
                if (_isServerConnected)
                {
                    _isServerConnectedText = "(connected)";
                }
                else
                {
                    _isServerConnectedText = "(not connected)";
                }
                OnPropertyChanged(nameof(IsServerConnected));
                OnPropertyChanged(nameof(IsServerConnectedText));
            }
        }

        private string _isServerConnectedText;

        public string IsServerConnectedText => _isServerConnectedText;

        private Visibility _isAuthOK;

        public Visibility IsAuthOK
        {
            get { return _isAuthOK; }
            set
            {
                _isAuthOK = value;
                OnPropertyChanged(nameof(IsAuthOK));
            }
        }

        private string _newPass;

        public string NewPass
        {
            get { return _newPass; }
            set
            {
                _newPass = value;
                OnPropertyChanged(nameof(NewPass));
            }
        }

        private string _newPassAgain;

        public string NewPassAgain
        {
            get { return _newPassAgain; }
            set
            {
                _newPassAgain = value;
                OnPropertyChanged(nameof(NewPassAgain));
            }
        }

        #region Messages And Visibility

        private string _connErrorMsg;

        public string ConnErrorMsg
        {
            get
            {
                return _connErrorMsg;
            }
            set
            {
                _connErrorMsg = value;
                OnPropertyChanged(nameof(ConnErrorMsg));
            }
        }

        private Brush _connErrorMsgColor;

        public Brush ConnErrorMsgColor
        {
            get
            {
                return _connErrorMsgColor;
            }
            set
            {
                _connErrorMsgColor = value;
                OnPropertyChanged(nameof(ConnErrorMsgColor));
            }
        }

        private Visibility _connErrorMsgVis;

        public Visibility ConnErrorMsgVis
        {
            get
            {
                return _connErrorMsgVis;
            }
            set
            {
                _connErrorMsgVis = value;
                OnPropertyChanged(nameof(ConnErrorMsgVis));
            }
        }

        private string _passErrorMsg;

        public string PassErrorMsg
        {
            get
            {
                return _passErrorMsg;
            }
            set
            {
                _passErrorMsg = value;
                OnPropertyChanged(nameof(PassErrorMsg));
            }
        }

        private Brush _passErrorMsgColor;

        public Brush PassErrorMsgColor
        {
            get
            {
                return _passErrorMsgColor;
            }
            set
            {
                _passErrorMsgColor = value;
                OnPropertyChanged(nameof(PassErrorMsgColor));
            }
        }

        private Visibility _passErrorMsgVis;

        public Visibility PassErrorMsgVis
        {
            get
            {
                return _passErrorMsgVis;
            }
            set
            {
                _passErrorMsgVis = value;
                OnPropertyChanged(nameof(PassErrorMsgVis));
            }
        }

        #endregion


        public SettingsViewModel(NavigationService navigationService)
        {
            SaveServerConnCmd = new ApplySettingsCmd(this);
            SaveNewPassCmd = new SaveNewPassCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            if (Stores.ConnectionStore.ConnectionStatus == null)
            {
                IsServerConnected = false;
            }
            else
            {
                IsServerConnected = (bool)Stores.ConnectionStore.ConnectionStatus;
            }
            IsMulticastEna = Properties.Settings.Default.IsMulticast;
            IsStaticIPEna = Properties.Settings.Default.IsStaticIP;
            IPAddress = Properties.Settings.Default.IPAddress;
        }
    }
}
