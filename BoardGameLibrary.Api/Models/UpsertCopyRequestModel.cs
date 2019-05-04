using System.ComponentModel.DataAnnotations;

namespace BoardGameLibrary.Api.Models
{
    public class UpsertCopyRequestModel
    {
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a Copy ID")]
        public string LibraryID { get; set; }
        public bool? Winnable { get; set; }
        public int? CollectionID { get; set; }
    }
}