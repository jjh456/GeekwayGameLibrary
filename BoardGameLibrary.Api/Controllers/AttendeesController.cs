using BoardGameLibrary.Data.Models;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using BoardGameLibrary.Api.Models;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using BoardGameLibrary.Api.Services;
using System.Web;

namespace BoardGameLibrary.Api.Controllers
{
    [RoutePrefix("Attendees")]
    public class AttendeesController : ApiController
    {
        private ApplicationDbContext _db;
        private IAttendeesFileUploadService uploadService;

        public AttendeesController()
        {
            _db = new ApplicationDbContext();
            uploadService = new AttendeesFileUploadService(_db);
        }

        [ResponseType(typeof(GetAttendeesResponseModel))]
        public IHttpActionResult Get()
        {
            var dbAttendees = _db.Attendees.OrderBy(a => a.Name);
            var response = MapDbAttendeesToResponse(dbAttendees);

            return Ok(response);
        }

        //GET attendees/5 || attendees? key = value
        public GetAttendeesResponseModel Get(string search)
        {
            var response = new GetAttendeesResponseModel();
            if (search == null)
                return response;

            IQueryable<Attendee> dbAttendees = _db.Attendees;

            if (!string.IsNullOrWhiteSpace(search))
                dbAttendees = dbAttendees.Where(a => a.Name.Contains(search) || a.BadgeID.Contains(search));

            response = MapDbAttendeesToResponse(dbAttendees);

            return response;
        }

        // GET: Attendees/5
        [ResponseType(typeof(Attendee))]
        public IHttpActionResult GetAttendee(string id)
        {
            Attendee attendee = _db.Attendees.FirstOrDefault(a => a.BadgeID == id);
            if (attendee == null)
                return NotFound();
            var apiModel = new AttendeeApiModel(attendee);

            return Ok(apiModel);
        }

        // PUT: Attendees/lib5
        [ResponseType(typeof(void))]
        [ScopeAuthorize("update:attendee")]
        public IHttpActionResult PutAttendee(string id, AttendeeApiModel apiModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbAttendee = _db.Attendees.FirstOrDefault(a => a.BadgeID == id);
            if (dbAttendee == null)
                return NotFound();

            if (id != dbAttendee.BadgeID)
                return BadRequest();
            dbAttendee.BadgeID = apiModel.BadgeNumber;
            dbAttendee.Name = apiModel.Name;

            _db.Entry(dbAttendee).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendeeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok(apiModel);
        }

        // POST: Attendees
        [ResponseType(typeof(Attendee))]
        [ScopeAuthorize("create:attendee")]
        public IHttpActionResult PostAttendee(AttendeeApiModel apiModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAttendee = _db.Attendees.FirstOrDefault(a => a.BadgeID == apiModel.BadgeNumber);
            if (existingAttendee != null)
                return BadRequest("There is already an attendee with that badge number.");

            var dbAttendee = new Attendee { BadgeID = apiModel.BadgeNumber, Name = apiModel.Name };

            _db.Attendees.Add(dbAttendee);
            _db.SaveChanges();
            apiModel.ID = dbAttendee.ID;

            return CreatedAtRoute("DefaultApi", new { id = apiModel.BadgeNumber }, apiModel);
        }

        [HttpPost]
        [Route("upload")]
        [ScopeAuthorize("create:attendee")]
        public IHttpActionResult UploadAttendees()
        {
            var files = HttpContext.Current.Request.Files;
            if (files == null || files.Count == 0)
                return BadRequest("You must provide a file");

            FileUploadResponse uploadResponse = uploadService.UploadAttendeesFile(files[0]);

            return Ok(uploadResponse);
        }

        // DELETE: Attendees/5
        [ResponseType(typeof(Attendee))]
        [ScopeAuthorize("delete:attendee")]
        public IHttpActionResult DeleteAttendee(string id)
        {
            Attendee dbAttendee = _db.Attendees.FirstOrDefault(a => a.BadgeID == id);
            if (dbAttendee == null)
                return NotFound();

            if (dbAttendee.Checkouts.Count > 0)
                return Conflict();

            _db.Attendees.Remove(dbAttendee);
            _db.SaveChanges();

            var apiModel = new AttendeeApiModel(dbAttendee);

            return Ok(apiModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();

            base.Dispose(disposing);
        }

        private bool AttendeeExists(string id)
        {
            return _db.Attendees.Count(e => e.BadgeID == id) > 0;
        }

        private GetAttendeesResponseModel MapDbAttendeesToResponse(IQueryable<Attendee> dbAttendees)
        {
            var response = new GetAttendeesResponseModel();
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
