using AppEvaluator.Commands;
using AppEvaluator.Commands.User;
using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using AppEvaluator.Stores;
using AppEvaluator.ViewModels.Teacher;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels.UserVMs
{
    internal class RunTestsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TestViewModel> _tests;
        private readonly List<SubjectViewModel> _subjects;

        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<SubjectViewModel> Subjects => _subjects;

        public ICommand SelectExecutableFileCmd { get; }
        public ICommand ReadDescriptionCmd { get; }
        public ICommand RunTestCmd { get; }
        public ICommand BackToMenuCmd { get; }

        private TestViewModel _selectedTest;

        public TestViewModel SelectedTest
        {
            get { return _selectedTest; }
            set { 
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        private FileStructure _selectedFile;

        public FileStructure SelectedFile
        {
            get { return _selectedFile; }
            set { 
                _selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
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
            set { 
                _fileContent = value;
                OnPropertyChanged(nameof(FileContent));
            }
        }

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

        public RunTestsViewModel(NavigationService navigationService)
        {
            SelectExecutableFileCmd = new SelectExecutableFileCmd(this);
            ReadDescriptionCmd = new ReadDescriptionCmd(this);
            RunTestCmd = new RunTestCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            _subjects = new List<SubjectViewModel>();
            _tests = new ObservableCollection<TestViewModel>();
            LoadSubjects();
        }

        /// <summary>
        /// Loads the tests from tha database that available for the user
        /// </summary>
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
        }
    }
}
