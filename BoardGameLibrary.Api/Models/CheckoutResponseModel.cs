using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Models
{
    public class CheckoutResponseModel
    {
        public int ID { get; set; }
        public CopyResponseModel Copy { get; set; }
        public AttendeeApiModel Attendee { get; set; }

        public CheckoutResponseModel(){}

        public CheckoutResponseModel(Checkout checkout)
        {
            ID = checkout.ID;
            Copy = new CopyResponseModel
            {
                ID = checkout.Copy.ID,
                Game = new GameResponseModel
                {
                    ID = checkout.Copy.Game.ID,
                    Name = checkout.Copy.Game.Title
                }
            };
            Attendee = new AttendeeApiModel
            {
                ID = checkout.Attendee.ID,
                BadgeNumber = checkout.Attendee.BadgeID,
                Name = checkout.Attendee.Name
            };
        }
    }
}