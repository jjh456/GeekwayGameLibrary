using BoardGameLibrary.Data.Models;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using BoardGameLibrary.Api.Models;

namespace BoardGameLibrary.Api.Controllers
{
    public class AttendeesController : ApiController
    {
        private ApplicationDbContext _db;

        public AttendeesController()
        {
            _db = new ApplicationDbContext();
        }

        //GET api/attendees/5 || api/attendees? key = value
        public GetAttendeesResponseModel Get(string search)
        {
            var response = new GetAttendeesResponseModel();
            if (search == null)
                return response;

            IQueryable<Attendee> dbAttendees = _db.Attendees;

            if (!string.IsNullOrWhiteSpace(search))
                dbAttendees = dbAttendees.Where(a => a.Name.Contains(search) || a.BadgeID.Contains(search));

            var attendees = new List<AttendeeApiModel>();
            attendees = dbAttendees
                .Select(dbAttendee => new AttendeeApiModel
                    {
                        BadgeNumber = dbAttendee.BadgeID,
                        ID = dbAttendee.ID,
                        Name = dbAttendee.Name
                    })
                .ToList();
            response.Attendees = attendees;

            return response;
        }
    }
}
