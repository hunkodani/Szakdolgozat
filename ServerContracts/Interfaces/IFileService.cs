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
        Task SaveTestFilesToServer(TestFileUpload testFileUpload);
        [OperationContract]
        Task SaveTestFilesToServerByName(TestFileUpload testFileUpload);
    }
}
