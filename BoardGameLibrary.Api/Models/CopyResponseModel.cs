using BoardGameLibrary.Data.Models;
using System;

namespace BoardGameLibrary.Api.Models
{
    public class CopyResponseModel
    {
        public string ID { get; set; }
        public bool IsCheckedOut { get; set; }
        public string Title { get; set; }
        public CheckoutResponseModel CurrentCheckout { get; set; }
        public GameResponseModel Game { get; set; }

        public CopyResponseModel(Copy copy)
        {
            ID = copy.LibraryID;
            IsCheckedOut = copy.CurrentCheckout != null;
            Title = copy.Title;
            if (IsCheckedOut)
                CurrentCheckout = new CheckoutResponseModel(copy.CurrentCheckout, false);
            Game = new GameResponseModel { ID = copy.Game.ID, Name = copy.Game.Title };
        }
    }
}