using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class GetAttendeesResponseModel
    {
        public IList<AttendeeApiModel> Attendees { get; set; }
    }
}