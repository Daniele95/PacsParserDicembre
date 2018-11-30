using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre
{
    public class QueryService : Publisher
    {
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public QueryService()
        {
        }

        public List<QueryObject> LaunchQuery(string dir, QueryObject obj,string service)
        {
            DicomToolkitQuery t = new DicomToolkitQuery(obj, dir, service);
            t.Event += onProcessEnd;
            t.launchProcess();

            manualResetEvent.Reset();
            manualResetEvent.WaitOne();

            // now parse xml files
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] Files = d.GetFiles("*.xml");
            List<QueryObject> queryResults = new List<QueryObject>();

            foreach (FileInfo file in Files)
            {
                QueryObject study = XmlTools.readDownloadedXml(dir + file.Name, obj);
                queryResults.Add(study);
            }

            return queryResults;
        }

        private void onProcessEnd(QueryObject s)
        {   
            manualResetEvent.Set();
        }

        public void readFile(object o, FileSystemEventArgs e)
        {
            Console.WriteLine("arrivato");
        }


    }

}
