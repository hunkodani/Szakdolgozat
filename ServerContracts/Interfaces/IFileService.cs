using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerContracts.Interfaces
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        Task SaveTestFilesToServer(FileUpload testFileUpload);
        [OperationContract]
        Task SaveTestFilesToServerByName(FileUpload testFileUpload);
        [OperationContract]
        Task<Stream> DownloadDescription(int testId);
        [OperationContract]
        Task<Stream> DownloadTestFile(string path);
        [OperationContract]
        List<string> GetTestFileNames(int testId);
    }
}
