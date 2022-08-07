using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Models
{
    [DataContract]
    public class Assignment
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int TestId { get; set; }

        public Assignment(int testId, int userId)
        {
            UserId = userId;
            TestId = testId;
        }
    }
}
