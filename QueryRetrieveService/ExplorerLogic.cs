using DataHandlerTools;
using PacsParserDicembre.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using EvilDICOM;
using System.Runtime.InteropServices;
using EvilDICOM.Core.Helpers;

namespace PacsParserDicembre
{
    public class ExplorerLogic
    {


        public List<QueryObject> searchPatient(StudyLevelQuery studyLevelQuery)
        {
            QueryService q = new QueryService();
            return q.LaunchQuery(Constants.queryResults, studyLevelQuery); 
        }

        public List<QueryObject> searchSeriesOfStudy(StudyLevelQuery studyLevelQuery)
        {
            QueryService q = new QueryService();
            SeriesLevelQuery qu = new SeriesLevelQuery(studyLevelQuery);
            return q.LaunchQuery(Constants.queryResults, qu);
        }

        public List<QueryObject> searchImage(SeriesLevelQuery s)
        {
            ImageLevelQuery qu = new ImageLevelQuery(s);
            QueryService q = new QueryService();
            List<QueryObject> queryResults = q.LaunchQuery(Constants.queryResults, qu);

            return queryResults;

        }

        public List<QueryObject> download(QueryObject info, string mode)
        {            
            DownloadManager d = new DownloadManager(info);
            List<QueryObject> files = d.LaunchQuery(mode);            
            return files;
        }

    }
}
