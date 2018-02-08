using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Models
{
    public class CopyResponseModel
    {
        public int ID { get; set; }
        public bool IsCheckedOut { get; set; }
        public GameResponseModel Game { get; set; }

        public CopyResponseModel(Copy copy)
        {
            ID = copy.LibraryID;
            IsCheckedOut = copy.CurrentCheckout != null;
            Game = new GameResponseModel { ID = copy.Game.ID, Name = copy.Game.Title };
        }
    }
}