using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacsParserDicembre.Tools
{

    class CommandLineProcess : Publisher
    {
        public Process proc;

        public void configureProcess(string[] queryString)
        {
            string arguments = queryString[1] + " -v";
            string queryType = queryString[0];

            proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Constants.DicomToolkit + queryType,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.EnableRaisingEvents = true;
        }

        public void launchProcess()
        {
            proc.Start();

            Thread thread = new Thread(new ThreadStart(logOutput));
            thread.Start();
        }

        void logOutput()
        {
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (Constants.Verbose) Console.WriteLine(line);
                if (line.Contains("(Success)"))
                    RaiseEvent(new StudyLevelQuery());
            }

        }
    }
}
