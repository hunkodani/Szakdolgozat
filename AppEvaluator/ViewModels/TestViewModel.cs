using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.ViewModels
{
    internal class TestViewModel: ViewModelBase
    {
        private readonly Test _test;

        public int TestId => _test.TestId;
        public string TestName => _test.TestName;
        public string SubjectCode => _test.SubjectCode;

        public TestViewModel(Test test)
        {
            _test = test;
        }
    }
}
