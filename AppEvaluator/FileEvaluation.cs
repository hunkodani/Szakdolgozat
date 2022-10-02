using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace AppEvaluator
{
    internal class FileEvaluation
    {
        private readonly static string testPrefix = "Test";
        private readonly int _numberOfCases;
        private readonly string _testName;
        private readonly string _path;

        public Process AppProcess { get; private set; }
        public string ResultPath { get; set; }

        public FileEvaluation(string path, int numberOfCases, string testName)
        {
            _path = path;
            _numberOfCases = numberOfCases;
            _testName = testName;
            ResultPath = "Result_" + _testName;
        }

        /// <summary>
        /// Calls the Evaluate method via a task
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {
            var task = Task.Run(() => Evaluate());
            await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new proccess with the given file and settings
        /// </summary>
        private void CreateNewProcess()
        {
            AppProcess = new Process();
            AppProcess.StartInfo.FileName = _path;
            AppProcess.StartInfo.UseShellExecute = false;
            AppProcess.StartInfo.RedirectStandardOutput = true;
            AppProcess.StartInfo.RedirectStandardInput = true;
            AppProcess.StartInfo.RedirectStandardError = true;
            AppProcess.StartInfo.CreateNoWindow = true;
        }

        /// <summary>
        /// Evaluates the executable file with the given test cases and creates a file from the result
        /// </summary>
        private void Evaluate()
        {
            bool isSuccess = true;
            double elapsedTime = 0.0;
            using (StreamWriter writer = new StreamWriter(ResultPath, false))
            {
                for (int i = 0; i < _numberOfCases; i++)
                {
                    using (StreamReader reader = new StreamReader(testPrefix + i + "_" + _testName))
                    {
                        string lineIN;
                        string lineOUT;
                        char commandType;
                        CreateNewProcess();
                        try
                        {
                            _ = AppProcess.Start();
                            while (!reader.EndOfStream)
                            {
                                lineIN = reader.ReadLine();
                                commandType = lineIN[0];
                                lineIN = lineIN.Remove(0, 2);
                                if (commandType == '>')
                                {
                                    AppProcess.StandardInput.WriteLine(lineIN);
                                }
                                else if (commandType == '<')
                                {
                                    lineOUT = AppProcess.StandardOutput.ReadLine();
                                    if (!lineIN.Equals(lineOUT))
                                    {
                                        throw new Exception();
                                    }
                                }
                                else
                                {

                                }
                            }
                            elapsedTime += AppProcess.UserProcessorTime.TotalMilliseconds;
                            if (!AppProcess.HasExited)
                            {
                                AppProcess.StandardInput.WriteLine();
                            }
                            if (!AppProcess.HasExited)
                            {
                                AppProcess.Close();
                            }
                            AppProcess.Dispose();

                            writer.WriteLine($"Evaluation case {i + 1} succeded.");
                        }
                        catch (Exception)
                        {
                            writer.WriteLine($"Evaluation failed at {i}. testcase. Further evaluation aborted");
                            isSuccess = false;
                            AppProcess?.Dispose();
                            break;
                        }
                    }
                }
                if (isSuccess)
                {
                    writer.WriteLine("\nEvaluation succeded. Everything passed.\nTotal time passed: " + elapsedTime + " ms, i.e: " + elapsedTime / 1000 + " sec.");
                }
            }
        }
    }
}
