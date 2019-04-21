namespace BoardGameLibrary.Api.Models
{
    public class UpsertCopyRequestModel
    {
        public string Title { get; set; }
        public string LibraryID { get; set; }
        public bool? Winnable { get; set; }
        public int? CollectionID { get; set; }
    }
}