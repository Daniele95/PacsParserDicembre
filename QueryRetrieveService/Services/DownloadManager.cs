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
        QueryObject downloadedFile; //currently downloaded

        public DownloadManager(QueryObject obj)
        {
            this.obj = obj;
        }

        public void LaunchQuery()
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

            foreach (string fileName in Files)
            {
                DicomToolkitToXML toXml = new DicomToolkitToXML(fileName);
                toXml.start();
                Thread.Sleep(50);
                QueryObject downloadFileInfo = XmlTools.readDownloadedXml(fileName + ".xml", new DownloadedFileInfo(),"download");
                File.Delete(fileName + ".xml");

                MessageBox.Show("ecco " + downloadFileInfo.GetField("SeriesInstanceUID"));
                storeInDatabase(fileName, downloadFileInfo);
            }
        }

        private void storeInDatabase(string filePath, QueryObject downloadedFile)
        {
            // store into database

            string folderStoragePath = Constants.database + "files/" + downloadedFile.GetField("PatientName") + " / " + downloadedFile.GetField("StudyDescription") + " / " + downloadedFile.GetField("SeriesDescription");
            System.IO.Directory.CreateDirectory(folderStoragePath);

            string fileStoragePath = folderStoragePath + "/" + downloadedFile.GetField("InstanceNumber") + ".dcm";
            downloadedFile.SetField("FileStoragePath", fileStoragePath);
            if (!File.Exists(fileStoragePath))
            {
                try
                {
                    File.Move(filePath, fileStoragePath);
                    database.Add(downloadedFile);
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
        }


        private void onProcessEnd(QueryObject s)
        {
            manualResetEvent.Set();
        }

    }
}
