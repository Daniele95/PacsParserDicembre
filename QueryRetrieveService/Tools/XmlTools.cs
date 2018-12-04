using PacsParserDicembre.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PacsParserDicembre
{
    public static class XmlTools
    {
        
        public static string getFolderStoragePath (QueryObject downloadedFile)
        {
            string folderStoragePath = Constants.database + "files/" + downloadedFile.GetField("PatientName") + "/" + downloadedFile.GetField("StudyDescription") + "/" + downloadedFile.GetField("SeriesDescription") + "/";
            return folderStoragePath;
        }

        public static string getStoragePath(QueryObject downloadedFile)
        {

            string folderStoragePath = getFolderStoragePath(downloadedFile);

            Directory.CreateDirectory(folderStoragePath);

            string fileStoragePath = folderStoragePath + "/" + downloadedFile.GetField("InstanceNumber") + ".dcm";
            return fileStoragePath;
        }


        public static QueryObject readDownloadedXml(string path, QueryObject downloadedFile,string option)
        {
            XmlDocument doc = new XmlDocument();
            QueryObject newFile = (QueryObject)Activator.CreateInstance(downloadedFile.GetType());
            List<string> queryKeys = newFile.getKeys();

            bool readFile = false;
            while(!readFile) {
                Thread.Sleep(50);
                try
                {
                    doc.Load(path);

                    foreach (string dicomTagName in queryKeys)
                    {
                        string result = findTag(doc, dicomTagName, option);

                        result = result.Replace("/", "-").Replace(' ', '_').Replace("\"", "-");
                        newFile.SetField(dicomTagName, result);
                    }
                    readFile = true;
                } catch(Exception)
                {
                    MessageBox.Show("Impossibile accedere al file " + path + ", ancora in scrittura");
                }
            }

            return newFile;
        }

        public static string findTag(XmlDocument doc, string dicomTagName,string option)
        {
            XmlNodeList xnList;
            string stringBefore = "";
            if (option == "find")
                stringBefore = "/data-set/element[@name='";
            if (option == "download")
                stringBefore = "/file-format/data-set/element[@name='";

            xnList = doc.SelectNodes(stringBefore + dicomTagName + "']");
            string result = "";
            if (xnList.Count > 0)
                result = xnList[0].InnerText;
            return result;
        }

    }
}
