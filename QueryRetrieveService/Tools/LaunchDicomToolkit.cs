using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PacsParserDicembre
{
    class LaunchDicomToolkit : Publisher
    {
        public Process proc;
        public LaunchDicomToolkit(QueryObject queryData, string dir)
        {
            configureProcess(queryString(queryData,dir));
        }

        // wrapper for Dicom Toolkit services
        string[] queryString(QueryObject queryData, string dir)
        {
            string query = "";
            foreach (string dicomTag in queryData.getKeys())
                query = " -k " + dicomTag + "=\"" + queryData.getValueByTag(dicomTag) + "\" " + query;


            string fullQuery =
                 " -S  -aec MIOSERVER " + query + " localhost 11112  -od " + dir + " --extract-xml ";


            DirectoryInfo di = Directory.CreateDirectory(dir);
            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            string[] queryString = { "findscu", fullQuery };
            return queryString;

        }

        void configureProcess(string[] queryString)
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
                if(line.Contains("(Success)"))
                    RaiseEvent(new StudyLevelQuery());
            }
            
        }
    }
}
