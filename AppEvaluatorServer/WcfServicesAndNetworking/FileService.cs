﻿using AppEvaluatorServer.FileManupulationAndSQL;
using ServerContracts.Interfaces;
using ServerContracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluatorServer.WcfServicesAndNetworking
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]//for debug: , IncludeExceptionDetailInFaults = true)]
    public class FileService : IFileService
    {
        /// <summary>
        /// Saves a file to the server by testId
        /// </summary>
        /// <param name="testFileUpload">The file to upload</param>
        /// <returns></returns>
        public async Task SaveTestFilesToServer(FileUpload testFileUpload)
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

        /// <summary>
        /// Saves a file to the server by TestName
        /// </summary>
        /// <param name="testFileUpload">The file to upload</param>
        /// <returns></returns>
        public async Task SaveTestFilesToServerByName(FileUpload testFileUpload)
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

        /// <summary>
        /// Download a test description file
        /// </summary>
        /// <param name="testId">The test's ID to download from</param>
        /// <returns>A stream to download</returns>
        public async Task<Stream> DownloadDescription(int testId)
        {
            string path = null;
            try
            {
                path = FileMethods.DataRoot;
                path = Path.Combine(path, SQLiteMethods.GetTestFolderLocation(testId));
                string fileName = null;
                if (!Directory.Exists(path))
                {
                    return Stream.Null;
                }
                foreach (string item in Directory.EnumerateFiles(path))
                {
                    string name = Path.GetFileName(item);
                    if (name.StartsWith("Desc_"))
                    {
                        fileName = name;
                    }
                }
                if (fileName == null)
                {
                    return null;
                }
                var task = Task.Run(() => { return DownloadFileStream(path, fileName); });
                return await task.ConfigureAwait(false);
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
                return null;
            }
        }

        /// <summary>
        /// Downloads a testfile
        /// </summary>
        /// <param name="path">A location to download from</param>
        /// <returns>A stream to download</returns>
        public async Task<Stream> DownloadTestFile(string path)
        {
            try
            {
                var task = Task.Run(() => { return DownloadFileStream(path); });
                return await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the test cases name
        /// </summary>
        /// <param name="testId">The test's ID for the searching</param>
        /// <returns>List of names to download</returns>
        public List<string> GetTestFileNames(int testId)
        {
            List<string> testFileNames = new List<string>();
            string path = null;
            try
            {
                path = FileMethods.DataRoot;
                path = Path.Combine(path, SQLiteMethods.GetTestFolderLocation(testId));
                if (!Directory.Exists(path))
                {
                    return testFileNames;
                }
                foreach (string item in Directory.EnumerateFiles(path))
                {
                    string name = Path.GetFileName(item);
                    if (!name.StartsWith("Desc_"))
                    {
                        testFileNames.Add(item);
                    }
                }
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
            return testFileNames;
        }

        /// <summary>
        /// Saves the Uploading file
        /// </summary>
        /// <param name="stream">The stream to upload</param>
        /// <param name="path">The path to put</param>
        /// <param name="fileName">The name of the file</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a download stream for a file
        /// </summary>
        /// <param name="path">Path to download from</param>
        /// <param name="fileName">The name of the file</param>
        /// <returns></returns>
        private Stream DownloadFileStream(string path, string fileName = null)
        {
            try
            {
                if (fileName != null)
                {
                    path = Path.Combine(path, fileName);
                }
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                return fs;
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
                return null;
            }
        }

        /// <summary>
        /// Downloads an evaluation file from the server
        /// </summary>
        /// <param name="relativePath">A path to download from</param>
        /// <returns>A stream to download</returns>
        public async Task<Stream> DownloadEvaluationFile(string relativePath)
        {
            string path = null;
            try
            {
                path = Path.Combine(FileMethods.DataRoot, relativePath);
                if (!File.Exists(path))
                {
                    return null;
                }
                var task = Task.Run(() => { return DownloadFileStream(path); });
                return await task.ConfigureAwait(false);
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
                return null;
            }
        }

        /// <summary>
        /// Uploads an evaluation file to the server
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <returns></returns>
        public async Task UploadEvaluationFile(FileUpload file)
        {
            string path = null;
            try
            {
                path = FileMethods.DataRoot;
                path = Path.Combine(path, file.ToRelativeLocation);
                var task = Task.Run(() => SaveFileStreamAsync(file.FileStreamer, path, file.FileName));
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
    }
}
