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
        public Process Process { get; private set; } = new Process();

        public FileEvaluation(string path, string filename)
        {
            Process.StartInfo.FileName = path + filename;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.CreateNoWindow = true;
        }

        public void Evaluate()
        {
            try
            {
                _ = Process.Start();
                while (true)  //itt nem lehet endofstream, mert akkor ha elfogy az olvasnivaló akkor leállna, függetlenül, hogy van-e még utána más
                {
                    if (Process.HasExited) //&& evaluation.Process.ExitCode > -1)   //Cannot put HasExited because it returns true if there is an input required (-xxxxxxxxx negative value)
                    {
                        while (!Process.StandardOutput.EndOfStream)
                        {
                            Process.StandardOutput.ReadLine();
                        }
                        break;
                    }
                    else
                    {
                        if (!(Process.StandardOutput.Peek() == -1))
                        {
                            Process.StandardOutput.ReadLine();
                        }
                        else
                        {
                            //billentyűlenyomást kellene átküldeni.... vagy: nem lehet a kódban readkey (a végén lehet, mert a rendszer leállítja, mert úgy érzékeli, mintha végzett volna)!!!
                            Process.StandardInput.WriteLine();
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                Process.Dispose();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (!Process.HasExited)
                {
                    Process.Close();
                }
                if (!Process.HasExited)
                {
                    Process.Kill();
                }
                Process.Dispose();
            }
        }
    }
}
