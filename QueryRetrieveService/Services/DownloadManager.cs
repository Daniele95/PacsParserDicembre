using Database;
using PacsParserDicembre;
using PacsParserDicembre.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace DataHandlerTools
{

    public class DownloadManager
    {
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        static ManualResetEvent eachFileWait = new ManualResetEvent(false);
        QueryObject obj;

        public DownloadManager(QueryObject obj)
        {
            this.obj = obj;
        }


        public List<QueryObject> LaunchQuery(string mode)
        {
            DicomToolkitDownload t = new DicomToolkitDownload(obj);
            t.Event += onProcessEnd;

            t.launchProcess();
            manualResetEvent.Reset();
            manualResetEvent.WaitOne();

            // now parse dicom files
            var allFiles = Directory.GetFiles(Constants.listenerFolder);
            var filesToExclude = Directory.GetFiles(Constants.listenerFolder,"*.xml");
            var Files = allFiles.Except(filesToExclude);

            List<QueryObject> downloadedFilesInfo = new List<QueryObject>();

            // iterate on images
            foreach (string fileName in Files)
            {
                DicomConvert.ToXml(fileName);

                QueryObject downloadFileInfo = XmlTools.readDownloadedXml(fileName + ".xml", new DownloadedFileInfo(),"download");
                File.Delete(fileName + ".xml");

                string folderStoragePath = "";
                if( mode == "series") folderStoragePath = storeInDatabase(fileName, downloadFileInfo);
                if( mode =="single") folderStoragePath = fileName;

                downloadedFilesInfo.Add(downloadFileInfo);
            }
            return downloadedFilesInfo;
        }


        private string storeInDatabase(string filePath, QueryObject downloadedFile)
        {
            string fileStoragePath = XmlTools.getStoragePath(downloadedFile);

            downloadedFile.SetField("FileStoragePath", fileStoragePath);

            if (!File.Exists(fileStoragePath))
            {
                try
                {
                    File.Move(filePath, fileStoragePath);
                    database.Add((DownloadedFileInfo)downloadedFile, Constants.database);
                    MessageBox.Show("File downloaded and stored in "+ fileStoragePath);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
            else
            {
                MessageBox.Show("file already present in database ");
            }
            File.Delete(filePath);

            return XmlTools.getFolderStoragePath(downloadedFile) + downloadedFile.GetField("InstanceNumber");
        }


        private void onProcessEnd(QueryObject s)
        {
            manualResetEvent.Set();
        }

    }
}
