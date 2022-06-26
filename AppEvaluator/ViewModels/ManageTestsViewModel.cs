using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppEvaluator.ViewModels
{
    internal class ManageTestsViewModel: ViewModelBase
    {
        private readonly ObservableCollection<TestViewModel> _tests;
        private readonly ObservableCollection<FileStructure> _files;

        public IEnumerable<TestViewModel> Tests => _tests;
        public IEnumerable<FileStructure> Files => _files;

        public ManageTestsViewModel()
        {
            _tests = new ObservableCollection<TestViewModel>();
            _files = new ObservableCollection<FileStructure>();

            _tests.Add(new TestViewModel(new Models.Test(1, "asd", "asd")));
            _tests.Add(new TestViewModel(new Models.Test(2, "asdasdasd", "asd")));
            _files.Add(new FileStructure("asd", "asdasd"));
        }

        public ICommand AddTestCommand { get; }
        public ICommand DeleteTestCommand { get; }
        public ICommand BackToMenuCommand { get; }

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

        private string _subjectCode;

        public string SubjectCode
        {
            get
            {
                return _subjectCode;
            }
            set
            {
                _subjectCode = value;
                OnPropertyChanged(nameof(SubjectCode));
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
