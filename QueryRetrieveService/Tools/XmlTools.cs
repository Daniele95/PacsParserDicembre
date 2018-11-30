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
    static class XmlTools
    {

        public static QueryObject readDownloadedXml(string path, QueryObject downloadedFile,string option)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            QueryObject newFile = (QueryObject)Activator.CreateInstance(downloadedFile.GetType());    

            List<string> queryKeys = newFile.getKeys();

            foreach (string dicomTagName in queryKeys)
            {
                string result = findTag(doc, dicomTagName,option);

                result = result.Replace(' ', '_').Replace("/", "-").Replace("\"", "-");
                newFile.SetField(dicomTagName, result);
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
