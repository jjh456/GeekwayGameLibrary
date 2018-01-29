namespace BoardGameLibrary.Api.Models
{
    public class CopyResponseModel
    {
        public int id { get; set; }
        public bool isCheckedOut { get; set; }
        public string gameTitle { get; set; }
    }
}