namespace BoardGameLibrary.Api.Models
{
    public class PostPlayerModel
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public bool WantsToWin { get; set; }

        public PostPlayerModel()
        {
            WantsToWin = true;
        }
    }
}