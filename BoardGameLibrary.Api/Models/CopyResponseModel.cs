namespace BoardGameLibrary.Api.Models
{
    public class CopyResponseModel
    {
        public int ID { get; set; }
        public bool IsCheckedOut { get; set; }
        public GameResponseModel Game { get; set; }
    }
}