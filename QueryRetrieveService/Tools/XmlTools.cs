using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PacsParserDicembre
{
    static class XmlTools
    {

        public static QueryObject readDownloadedXml(string path, StudyLevelQuery downloadedFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            StudyLevelQuery newFile = new StudyLevelQuery();

                newFile.SetField("PatientName", findTag(doc, "PatientName"));
                newFile.SetField("StudyDescription", findTag(doc, "StudyDescription"));
                newFile.SetField("SeriesDescription", findTag(doc, "SeriesDescription"));

                return newFile;
        }

        static string findTag(XmlDocument doc, string dicomTagName)
        {

            XmlNodeList xnList;
            xnList = doc.SelectNodes("/data-set/element[@name='" + dicomTagName + "']");
            string result = "";
            if (xnList.Count > 0)
                result = xnList[0].InnerText;
            return result;
        }


    }
}
