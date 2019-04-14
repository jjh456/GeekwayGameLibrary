using CsvHelper.Configuration.Attributes;

namespace BoardGameLibrary.Api.Models
{
    public class AttendeeUploadRow
    {
        [Index(0)]
        public string Name { get; set; }
        [Index(1)]
        public string BadgeID { get; set; }
    }
}