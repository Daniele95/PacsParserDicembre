using PacsParserDicembre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.Windows;

namespace Database
{

    public class DownloadedFileInfo : QueryObject
    {
        [BsonId]
        public string SOPInstanceUID { get; set; } = "";

        public string SeriesInstanceUID { get; set; } = "";
        public string SeriesDescription { get; set; } = "";
        public string InstanceNumber { get; set; } = "";
        public string PatientName { get; set; } = "";
        public string StudyDescription { get; set; } = "";
        public string StudyDate { get; set; } = "";
        public string Modality { get; set; } = "";
    }


    public static class database
    {

        public static List<DownloadedFileInfo> Get(DownloadedFileInfo fileToDownload, string databaseFolder)
        {
            using (var db = new LiteDatabase(databaseFolder + "database.db"))
            {
                var collection = db.GetCollection<DownloadedFileInfo>("downloadedFiles");
                var results = collection.Find(x =>  
                x.PatientName.Contains(fileToDownload.GetField("PatientName"))
                );
                List<DownloadedFileInfo> res = new List<DownloadedFileInfo>(results);
                foreach (DownloadedFileInfo result in res)
                    result.fill(); // fills internal dictionary
                return res;
            }
        }

        public static void Add(DownloadedFileInfo aDownloadedFile,string databaseFolder)
        {
            using (var db = new LiteDatabase(databaseFolder + "database.db"))
            {
                var downloadedFiles = db.GetCollection<DownloadedFileInfo>("downloadedFiles");
                downloadedFiles.Insert(aDownloadedFile);
                downloadedFiles.Update(aDownloadedFile);

                downloadedFiles.EnsureIndex(x => x.SOPInstanceUID);
                DownloadedFileInfo result =downloadedFiles.FindById(aDownloadedFile.SOPInstanceUID);

            }
        }
    }
}
