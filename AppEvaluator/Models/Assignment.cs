using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Models
{
    internal class Assignment
    {
        public int UserId { get; }
        public int TestId { get; }

        public Assignment(int testId, int userId)
        {
            UserId = userId;
            TestId = testId;
        }
    }
}
