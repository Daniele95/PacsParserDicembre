using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitToXML : Publisher
    {
        string[] queryString;
        public CommandLineProcess ComandLineTool = new CommandLineProcess();
        string path;
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);


        public DicomToolkitToXML(string path)
        {
            this.path = path;
            string arguments = " " + path + " " + path + ".xml";
            string[]  comand = { "dcm2xml", arguments };
            queryString = comand;
            ComandLineTool.configureProcess(queryString);
        }
        


        public void start()
        {
            ComandLineTool.launchProcess();
        }

    }
}
