using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer
{
    internal class Assignment
    {
        public int UserId { get; set; }
        public int TestId { get; set; }

        public Assignment(int testId, int userId)
        {
            UserId = userId;
            TestId = testId;
        }
    }
}
