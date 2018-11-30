using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitDownload : DicomToolkitAbstractQuery
    {
        public DicomToolkitDownload(QueryObject queryData) : base(queryData, "") {  }

        public override string[] specificQuery(string mainQuery,string dir)
        {
            string fullQuery = " -aem USER  -aec MIOSERVER " + mainQuery + " localhost 11112";
            string[] queryString = { "movescu", fullQuery };
            return queryString;
        }

    }
}
