using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitQuery : DicomToolkitAbstractQuery
    {
        public DicomToolkitQuery(QueryObject queryData,string dir) : base(queryData, dir) {}

        public override string[] specificQuery(string mainQuery, string dir)
        {
            string fullQuery = " -S  -aec "+ Constants.AECalled+" " + mainQuery + " "+
                Constants.serverIp+" " +Constants.serverPort+"  -od " + dir + " --extract-xml ";
            string[] queryString = { "findscu", fullQuery };
            return queryString;
        }
    }
}
