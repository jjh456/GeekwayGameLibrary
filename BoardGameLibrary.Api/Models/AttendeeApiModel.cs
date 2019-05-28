using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Models
{
    public class AttendeeApiModel
    {
        public int ID { get; set; }
        public string BadgeNumber { get; set; }
        public string Name { get; set; }

        public AttendeeApiModel() { }
        public AttendeeApiModel(Attendee attendee)
        {
            ID = attendee.ID;
            BadgeNumber = attendee.BadgeID;
            Name = attendee.Name;
        }
    }
}