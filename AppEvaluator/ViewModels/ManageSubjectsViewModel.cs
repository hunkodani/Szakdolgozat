using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppEvaluator.ViewModels
{
    internal class ManageSubjectsViewModel: ViewModelBase
    {
        private readonly ObservableCollection<SubjectViewModel> _subjects;

        public IEnumerable<SubjectViewModel> Subjects => _subjects;

        public ManageSubjectsViewModel()
        {
            _subjects = new ObservableCollection<SubjectViewModel>();

            _subjects.Add(new SubjectViewModel(new Models.Subject("asd", "asd", "asd")));
            _subjects.Add(new SubjectViewModel(new Models.Subject("asdasdasd", "asdasdasd", "asd")));
        }

        public ICommand AddSubjectCommand { get; }
        public ICommand DeleteSubjectCommand { get; }
        public ICommand BackToMenuCommand { get; }

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

        private string _subjectName;

        public string SubjectName
        {
            get 
            { 
                return _subjectName;
            }
            set 
            { 
                _subjectName = value;
                OnPropertyChanged(nameof(SubjectName));
            }
        }
    }
}
