using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer
{
    internal class Subject
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string FolderLocation { get; set; }

        public Subject(string subjectCode, string subjectName, string folderLocation)
        {
            SubjectCode = subjectCode;
            SubjectName = subjectName;
            FolderLocation = folderLocation;
        }
    }
}
