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

        public async Task Execute()
        {
            var task = Task.Run(() => Evaluate());
            await task.ConfigureAwait(false);
        }

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
                            elapsedTime += AppProcess.UserProcessorTime.TotalMilliseconds;//AppProcess.TotalProcessorTime.TotalMilliseconds;
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

        /*private bool ExecuteProgram(StreamReader reader)
        {
            try
            {
                _ = AppProcess.Start();
                while (true)  //itt nem lehet endofstream, mert akkor ha elfogy az olvasnivaló akkor leállna, függetlenül, hogy van-e még utána más
                {
                    if (AppProcess.HasExited) //&& evaluation.Process.ExitCode > -1)   //Cannot put HasExited because it returns true if there is an input required (-xxxxxxxxx negative value)
                    {
                        while (!AppProcess.StandardOutput.EndOfStream)
                        {
                            AppProcess.StandardOutput.ReadLine();
                        }
                        break;
                    }
                    else
                    {
                        if (AppProcess.StandardOutput.Peek() != -1)
                        {
                            AppProcess.StandardOutput.ReadLine();
                        }
                        else
                        {
                            //billentyűlenyomást kellene átküldeni.... vagy: nem lehet a kódban readkey (a végén lehet, mert a rendszer leállítja, mert úgy érzékeli, mintha végzett volna)!!!
                            AppProcess.StandardInput.WriteLine("1");
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                if (!AppProcess.HasExited)
                {
                    AppProcess.Close();
                }
                if (!AppProcess.HasExited)
                {
                    AppProcess.Kill();
                }
                AppProcess.Dispose();
                return false;
            }
            catch (Exception)
            {
                if (!AppProcess.HasExited)
                {
                    AppProcess.Close();
                }
                if (!AppProcess.HasExited)
                {
                    AppProcess.Kill();
                }
                AppProcess.Dispose();
                return false;
            }
            if (!AppProcess.HasExited)
            {
                AppProcess.Close();
            }
            if (!AppProcess.HasExited)
            {
                AppProcess.Kill();
            }
            AppProcess.Dispose();
            return true;
        }*/
    }
}
