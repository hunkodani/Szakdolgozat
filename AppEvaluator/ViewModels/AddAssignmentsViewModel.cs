using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AppEvaluator.ViewModels
{
    internal class AddAssignmentsViewModel
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
        public ICommand BackToMenuCommand { get; }

        public AddAssignmentsViewModel()
        {
            _users = new ObservableCollection<UserAssignmentViewModel>();
            _tests = new ObservableCollection<TestViewModel>();

            _tests.Add(new TestViewModel(new Models.Test(1, "asd", "asd")));
            _tests.Add(new TestViewModel(new Models.Test(2, "asdasdasd", "asd")));
            _tests.Add(new TestViewModel(new Models.Test(3, "asdsd", "as")));
            _tests.Add(new TestViewModel(new Models.Test(4, "asdasdasd", "as")));

            _users.Add(new UserAssignmentViewModel(new User("asd", "asd", "asd", 1, "asd", 1)));
            _users.Add(new UserAssignmentViewModel(new User("asdasd", "asd", "assd", 1, "asssd", 2)));
            _users.Add(new UserAssignmentViewModel(new User("dasd", "asd", "sd", 1, "asssd", 3)));
        }
    }
}
