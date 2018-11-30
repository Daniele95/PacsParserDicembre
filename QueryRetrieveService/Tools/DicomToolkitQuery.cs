﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitQuery : DicomToolkitAbstractQuery
    {
        public DicomToolkitQuery(QueryObject queryData,string dir) : base(queryData, dir) {

            MessageBox.Show("caioo");
        }

        public override string[] specificQuery(string mainQuery, string dir)
        {
            string fullQuery = " -S  -aec MIOSERVER " + mainQuery + " localhost 11112  -od " + dir + " --extract-xml ";
            string[] queryString = { "findscu", fullQuery };
            MessageBox.Show(fullQuery);
            return queryString;
        }
    }
}
