using CsvHelper.Configuration.Attributes;

namespace BoardGameLibrary.Api.Models
{
    public class CopyUploadRow
    {
        [Index(0)]
        public string GameTitle { get; set; }
        [Index(1)]
        public string LibraryID { get; set; }
        [Index(2)]
        public string OwnerName { get; set; }
    }
}