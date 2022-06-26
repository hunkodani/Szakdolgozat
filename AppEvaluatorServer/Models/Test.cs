using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer
{
    internal class Test
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string SubjectCode { get; set; }

        public Test(int testId, string testName, string subjectCode)
        {
            TestId = testId;
            TestName = testName;
            SubjectCode = subjectCode;
        }
    }
}
