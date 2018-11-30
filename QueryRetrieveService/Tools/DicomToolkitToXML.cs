using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitToXML : Publisher
    {
        string[] queryString;
        public CommandLineProcess ComandLineTool;
        string path;
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);


        public DicomToolkitToXML(string path)
        {
            this.path = path;
            string arguments = " " + path + " " + path + ".xml";
            string[]  comand = { "dcm2xml", arguments };
            queryString = comand;
            ComandLineTool.Event += onConverted;
            ComandLineTool = new CommandLineProcess();
            ComandLineTool.configureProcess(queryString);
        }

        private void onConverted(QueryObject obj)
        {
            QueryObject downloadFileInfo = XmlTools.readDownloadedXml(path, new DownloadedFileInfo());
            RaiseEvent(downloadFileInfo);
        }


        public void start()
        {
            ComandLineTool.launchProcess();
        }

    }
}
