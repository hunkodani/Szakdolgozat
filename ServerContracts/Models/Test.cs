using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Models
{
    [DataContract]
    public class Test
    {
        [DataMember]
        public int TestId { get; set; }
        [DataMember]
        public string TestName { get; set; }
        [DataMember]
        public string SubjectCode { get; set; }

        public Test(int testId, string testName, string subjectCode)
        {
            TestId = testId;
            TestName = testName;
            SubjectCode = subjectCode;
        }
    }
}
