using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitDownload : DicomToolkitQuery
    {
        public DicomToolkitDownload(QueryObject queryData, string dir) : base(queryData, dir) { }

        new string [] specificQuery(string mainQuery, string dir)
        {
             string fullQuery = " -aem USER  -aec MIOSERVER " + mainQuery + " localhost 11112";
            string[] queryString = { "movescu", fullQuery };
            return queryString;
        }

    }
}
