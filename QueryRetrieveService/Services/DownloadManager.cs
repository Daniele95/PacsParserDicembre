using Database;
using LiteDB;
using PacsParserDicembre;
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

        public DownloadManager(string fullPath)
        {

        }


        public void onCreated(object o, FileSystemEventArgs e)
        {

            //  queryTools.IsFileClosed(e.FullPath, true);
            string[] splitted = e.FullPath.Split('.');
            string extension = splitted[splitted.Length - 1];
            if (extension == "part")
            {
                string filePath = e.FullPath.Substring(0, e.FullPath.Length - 5); //whithout extension .part
                while (!File.Exists(filePath)) { }
                dicom2xml(filePath);
            }
            else // if xml: read it  and store \and store info into database
            {
                DownloadedFileInfo downloadedFile = readDownloadedXml(e.FullPath);

                string folderStoragePath = Constants.database + "files/" + downloadedFile.PatientName + "/" + downloadedFile.StudyDescription + "/" + downloadedFile.SeriesDescription;
                System.IO.Directory.CreateDirectory(folderStoragePath);

                string fileStoragePath = folderStoragePath + "/" + downloadedFile.InstanceNumber + ".dcm";
                downloadedFile.FileStoragePath = fileStoragePath;
                string filePath = e.FullPath.Substring(0, e.FullPath.Length - 4); //whithout extension .xml
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
        }

        public void onSeriesButtonPressed(SeriesLevelQuery queryResults)
        {

            DicomToolkitQuery t = new DicomToolkitQuery(queryResults, dir, service);
            t.Event += onProcessEnd;
            t.launchProcess();

            manualResetEvent.Reset();
            manualResetEvent.WaitOne();
        }

        public static void dicom2xml(string path)
        {
            string arguments = " " + path + " " + path + ".xml";
            queryTools.initProcess("dcm2xml", arguments);
        }

        public static DownloadedFileInfo readDownloadedXml(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            DownloadedFileInfo downloadedFile = new DownloadedFileInfo();
            foreach (string dicomTag in downloadedFile.getKeys())
            {
                if (dicomTag != "FileStoragePath")
                {
                    string myTag = findTag(doc, dicomTag);
                    myTag = myTag.Replace(' ', '_');
                    myTag = myTag.Replace("/", "-");
                    myTag = myTag.Replace("\"", "-");
                    downloadedFile.SetField(dicomTag, myTag);
                }
            }
            File.Delete(path);
            return downloadedFile;
        }


        static string findTag(XmlDocument doc, string dicomTagName)
        {

            XmlNodeList xnList;
            xnList = doc.SelectNodes("/file-format/data-set/element[@name='" + dicomTagName + "']");
            string result = "";
            if (xnList.Count > 0)
                result = xnList[0].InnerText;
            return result;
        }

    }
}
