using ServerContracts.Interfaces;
using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer.WcfServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]//for debug: , IncludeExceptionDetailInFaults = true)]
    public class FileService : IFileService
    {
        public async Task SaveTestFilesToServer(TestFileUpload testFileUpload)
        {
            string path = null;
            try
            {
                path = FileMethods.DataRoot;
                path = Path.Combine(path, SQLiteMethods.GetTestFolderLocation(testFileUpload.TestId));
                var task = Task.Run(() => SaveFileStreamAsync(testFileUpload.FileStreamer, path, testFileUpload.FileName));
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (path == null || path == String.Empty)
                {
                    Logging.WriteToLog(LogTypes.Error, "No DataRoot was set, no file movemement can happen.");
                }
                else
                {
                    Logging.WriteToLog(LogTypes.Error, e.Message);
                }
            }
        }

        public async Task SaveTestFilesToServerByName(TestFileUpload testFileUpload)
        {
            string path = null;
            try
            {
                path = FileMethods.DataRoot;
                path = Path.Combine(path, SQLiteMethods.GetTestFolderLocation(testFileUpload.TestName));
                var task = Task.Run(() => SaveFileStreamAsync(testFileUpload.FileStreamer, path, testFileUpload.FileName));
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (path == null || path == String.Empty)
                {
                    Logging.WriteToLog(LogTypes.Error, "No DataRoot was set, no file movemement can happen.");
                }
                else
                {
                    Logging.WriteToLog(LogTypes.Error, e.Message);
                }
            }
        }

        private async Task SaveFileStreamAsync(Stream stream, string path, string fileName) 
        {
            try
            {
                using (stream)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, fileName);
                    using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(file);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
            }
        }
    }
}
