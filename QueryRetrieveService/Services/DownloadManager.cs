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
        QueryObject downloadedFile; //currently downloaded


        public void LaunchQuery(string dir, QueryObject obj)
        {
            DicomToolkitDownload t = new DicomToolkitDownload(obj, dir);
            t.Event += onProcessEnd;

            t.launchProcess();
            manualResetEvent.Reset();
            manualResetEvent.WaitOne();

            // now parse dicom files
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] Files = d.GetFiles();

            foreach (FileInfo file in Files)
            {
                DicomToolkitToXML toXml = new DicomToolkitToXML(file.FullName);
                toXml.Event += onConverted;
                toXml.start();

                eachFileWait.Reset();
                eachFileWait.WaitOne();

                storeInDatabase(file.FullName);
            }
        }
        private void onConverted (QueryObject downloadedFile)
        {
            this.downloadedFile = downloadedFile;
            eachFileWait.Set();

        }

        private void storeInDatabase(string fullPath)
        {
            // store into database

            string folderStoragePath = Constants.database + "files/" + downloadedFile.GetField("PatientName") + " / " + downloadedFile.GetField("StudyDescription") + " / " + downloadedFile.GetField("SeriesDescription");
            System.IO.Directory.CreateDirectory(folderStoragePath);

            string fileStoragePath = folderStoragePath + "/" + downloadedFile.GetField("InstanceNumber") + ".dcm";
            downloadedFile.SetField("FileStoragePath", fileStoragePath);

            string filePath = fullPath.Substring(0, fullPath.Length - 4); //whithout extension .xml
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
