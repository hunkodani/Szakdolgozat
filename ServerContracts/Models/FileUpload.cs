using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Models
{
    [MessageContract]
    public class FileUpload
    {
        [MessageHeader(MustUnderstand = true)]
        public int TestId { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string TestName { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string FileName { get; set; }
        [MessageBodyMember]
        public Stream FileStreamer { get; set; }

        public FileUpload(int testId, string fileName, Stream fileStreamer)
        {
            TestId = testId;
            FileName = fileName;
            FileStreamer = fileStreamer;
        }

        public FileUpload(string testName, string fileName, Stream fileStreamer)
        {
            TestName = testName;
            FileName = fileName;
            FileStreamer = fileStreamer;
        }

        public FileUpload()
        {
        }
    }
}
