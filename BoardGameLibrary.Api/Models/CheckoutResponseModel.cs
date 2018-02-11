using BoardGameLibrary.Data.Models;
using System;

namespace BoardGameLibrary.Api.Models
{
    public class CheckoutResponseModel
    {
        public int ID { get; set; }
        public CopyResponseModel Copy { get; set; }
        public AttendeeApiModel Attendee { get; set; }
        public CheckoutLengthModel CheckoutLength { get; set; }

        public CheckoutResponseModel(){}

        public CheckoutResponseModel(Checkout checkout, bool includeCopy = true)
        {
            ID = checkout.ID;
            if (includeCopy)
            {
                Copy = new CopyResponseModel(checkout.Copy)
                {
                    ID = checkout.Copy.LibraryID,
                    Game = new GameResponseModel
                    {
                        ID = checkout.Copy.Game.ID,
                        Name = checkout.Copy.Game.Title
                    }
                };
            }
            Attendee = new AttendeeApiModel
            {
                ID = checkout.Attendee.ID,
                BadgeNumber = checkout.Attendee.BadgeID,
                Name = checkout.Attendee.Name
            };
            CheckoutLength = new CheckoutLengthModel {
                Days = checkout.Length.Days,
                Hours = checkout.Length.Hours,
                Minutes = checkout.Length.Minutes,
                Seconds = checkout.Length.Seconds
            };
        }
    }
}