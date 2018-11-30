using PacsParserDicembre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Database
{

    public class DownloadedFileInfo : QueryObject
    {
        public string PatientName { get; set; } = "";
        public string StudyDescription { get; set; } = "";
        [BsonId]
        public string SeriesInstanceUID { get; set; } = "";
        public string SeriesDescription { get; set; } = "";
        public string InstanceNumber { get; set; } = "";
    }


    public static class database
    {

        public static void Add(QueryObject aDownloadedFile,string databaseFolder)
        {
            using (var db = new LiteDatabase(databaseFolder + "database.db"))
            {
                var downloadedFiles = db.GetCollection<QueryObject>("downloadedFiles");
                downloadedFiles.Insert(aDownloadedFile);
                downloadedFiles.Update(aDownloadedFile);
            }
        }
    }
}
