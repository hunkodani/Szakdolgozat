using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Models
{
    internal class Test
    {
        public int TestId { get; }
        public string TestName { get; set; }
        public string SubjectCode { get; }

        public Test(int testId, string testName, string subjectCode)
        {
            TestId = testId;
            TestName = testName;
            SubjectCode = subjectCode;
        }
    }
}
