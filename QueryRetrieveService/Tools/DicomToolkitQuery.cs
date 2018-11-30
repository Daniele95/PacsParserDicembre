using PacsParserDicembre.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PacsParserDicembre
{
    class DicomToolkitQuery : CommandLineProcess
    {
        public DicomToolkitQuery(QueryObject queryData, string dir)
        {
            string[] query = queryString(queryData, dir);
            configureProcess(query);
        }

        // wrapper for Dicom Toolkit services
        string[] queryString(QueryObject queryData, string dir)
        {
            string query = "";
            foreach (string dicomTag in queryData.getKeys())
                query = " -k " + dicomTag + "=\"" + queryData.GetField(dicomTag) + "\" " + query;

            DirectoryInfo di = Directory.CreateDirectory(dir);
            foreach (FileInfo file in di.GetFiles())
                file.Delete();
            
            return specificQuery(query,dir);

        }

        string [] specificQuery(string mainQuery, string dir)
        {
            string fullQuery = " -S  -aec MIOSERVER " + mainQuery + " localhost 11112  -od " + dir + " --extract-xml ";
            string[] queryString = { "findscu", fullQuery };
            return queryString;
        }


        
    }
}
