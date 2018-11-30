using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacsParserDicembre.Tools
{
    class DicomToolkitDownload : DicomToolkitQuery
    {
        
            if (service == "move")
            {
                fullQuery = " -aem USER  -aec MIOSERVER " + query + " localhost 11112";
                queryString[0] = "movescu";
            }

}
}
