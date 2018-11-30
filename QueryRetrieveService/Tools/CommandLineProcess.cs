using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre.Tools
{

    static class DicomConvert
    {
        public static void ToXml(string fileName)
        {
            string arguments = " " + fileName + " " + fileName + ".xml";
            StaticCommandLineProcess.Start("dcm2xml", arguments);
            Thread.Sleep(50);
        }
        public static void ToJpg(string fileName)
        {
            string arguments = " " + fileName + ".dcm " + fileName + ".jpeg";
            StaticCommandLineProcess.Start("toJpg/dcmcjpeg", arguments);
            Thread.Sleep(50);
        }

    }

    static class StaticCommandLineProcess
    {
        public static void Start(string process, string arguments)
        {
            CommandLineProcess c = new CommandLineProcess();
            string[] queryString = { process, arguments };
            c.configureProcess(queryString);
            c.launchProcess();
        }
    }

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
                if (line.Contains("Success"))
                {
                    RaiseEvent(new StudyLevelQuery());
                }
            }

        }
    }
}
