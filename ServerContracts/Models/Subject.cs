using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Models
{
    [DataContract]
    public class Subject
    {
        [DataMember]
        public string SubjectCode { get; set; }
        [DataMember]
        public string SubjectName { get; set; }
        [DataMember]
        public string FolderLocation { get; set; }

        public Subject(string subjectCode, string subjectName, string folderLocation)
        {
            SubjectCode = subjectCode;
            SubjectName = subjectName;
            FolderLocation = folderLocation;
        }
    }
}
