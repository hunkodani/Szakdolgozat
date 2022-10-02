using AppEvaluator.Commands;
using AppEvaluator.Commands.Teacher;
using AppEvaluator.Models;
using AppEvaluator.NetworkingAndWCF;
using AppEvaluator.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace AppEvaluator.ViewModels.Teacher
{
    internal class ManageTestsViewModel: ViewModelBase
    {
        private readonly ObservableCollection<TestViewModel> _tests;
        private ObservableCollection<FileStructure> _files;
        private readonly List<SubjectViewModel> _cBSubjects;

        internal ObservableCollection<FileStructure> TestFiles { get => _files; set => _files = value; }

        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<FileStructure> Files => _files;
        public IEnumerable<SubjectViewModel> CBSubjects => _cBSubjects;

        public ManageTestsViewModel(NavigationService navigationService)
        {
            AddTestCmd = new AddTestCmd(this);
            DeleteTestCmd = new DeleteTestCmd(this);
            LoadTestsCmd = new LoadTestsCmd(this);
            AddTestFileCmd = new AddTestFileCmd(this);
            AddDescFileCmd = new AddDescFileCmd(this);
            RemoveTestFileCmd = new RemoveTestFileCmd(this);
            BackToMenuCmd = new NavigateCmd(navigationService);
            _tests = new ObservableCollection<TestViewModel>();
            _files = new ObservableCollection<FileStructure>();
            _cBSubjects = new List<SubjectViewModel>();

            LoadSubjects();
        }

        public ICommand AddTestCmd { get; }
        public ICommand DeleteTestCmd { get; }
        public ICommand LoadTestsCmd { get; }
        public ICommand AddTestFileCmd { get; }
        public ICommand AddDescFileCmd { get; }
        public ICommand RemoveTestFileCmd { get; }
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

        private string _testName;

        public string TestName
        {
            get
            {
                return _testName;
            }
            set
            {
                _testName = value;
                OnPropertyChanged(nameof(TestName));
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
            }
        }

        private SubjectViewModel _cBSelectedSubject;

        public SubjectViewModel CBSelectedSubject
        {
            get 
            { 
                return _cBSelectedSubject; 
            }
            set 
            {
                _cBSelectedSubject = value;
                OnPropertyChanged(nameof(CBSelectedSubject));
            }
        }

        private FileStructure _descFile;

        public FileStructure DescFile
        {
            get 
            { 
                return _descFile; 
            }
            set 
            { 
                _descFile = value;
                OnPropertyChanged(nameof(DescFile));
            }
        }

        private TestViewModel _selectedTest;
        public TestViewModel SelectedTest
        {
            get
            {
                return _selectedTest;
            }
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        /// <summary>
        /// Loads the selected subject's tests from the database
        /// </summary>
        internal void LoadTests()
        {
            List<Test> tests = null;
            if (_cBSelectedSubject != null)
            {
                tests = WcfDataParser.TestsParse(WcfService.MainProxy?.GetTests(_cBSelectedSubject.Code));
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
            _cBSubjects.Clear();
            if (subjects != null)
            {
                foreach (Subject subject in subjects)
                {
                    _cBSubjects.Add(new SubjectViewModel(subject));
                }
            }
        }
    }

    internal class FileStructure
    {
        public string Name { get; }
        public string Location { get; }

        public FileStructure(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}
