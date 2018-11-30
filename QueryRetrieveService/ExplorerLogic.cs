using DataHandlerTools;
using PacsParserDicembre.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre
{
    public class ExplorerLogic
    {

        public List<QueryObject> searchPatient()
        {
            ImageLevelQuery qu = new ImageLevelQuery();
            qu.SetField("SeriesInstanceUID", "1.3.6.1.4.1.5962.1.1.0.0.0.1168612284.20369.0.2");
            qu.SetField("StudyInstanceUID", "1.3.6.1.4.1.5962.1.1.0.0.0.1168612284.20369.0.1");
            QueryService q = new QueryService();
            List<QueryObject> queryResults = q.LaunchQuery(@"C:\Users\daniele\Desktop\QUERYRESULTS\", qu);

            return queryResults;

        }

        public void download()
        {            
            SeriesLevelQuery info = new SeriesLevelQuery();
            info.SetField("StudyInstanceUID", "1.3.6.1.4.1.5962.1.1.0.0.0.1168612284.20369.0.1");
            info.SetField("SeriesInstanceUID", "1.3.6.1.4.1.5962.1.1.0.0.0.1168612284.20369.0.2");
            info.SetField("PatientID", "TEST2351267");

            DownloadManager d = new DownloadManager(info);
            List<string> files = d.LaunchQuery();


            DicomConvert.ToJpg(files[0]);

        }

    }
}
