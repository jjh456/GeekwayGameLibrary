using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using BoardGameLibrary.Api.Models;
using System;
using System.Data.Entity;

namespace BoardGameLibrary.Api.Controllers
{
    public class CheckoutsController : ApiController
    {
        private ApplicationDbContext _db;

        public CheckoutsController()
        {
            _db = new ApplicationDbContext();
        }

        //GET api/checkouts
        public IEnumerable<Checkout> Get() => _db.Checkouts.ToList();

        //GET api/checkouts/5 || api/checkouts? key = value
        public IEnumerable<CheckoutResponseModel> Get(string badgeId)
        {
            if (badgeId == null)
                return null;

            return _db.Checkouts
                .Where(co => co.Attendee.BadgeID == badgeId && co.Play == null)
                .Select(co => new CheckoutResponseModel
                {
                    ID = co.ID,
                    Copy = new CopyResponseModel
                    {
                        ID = co.Copy.ID,
                        Game = new GameResponseModel
                        {
                            ID = co.Copy.Game.ID,
                            Name = co.Copy.Game.Title
                        }
                    },
                    Attendee = new AttendeeApiModel
                    {
                        ID = co.Attendee.ID,
                        BadgeNumber = co.Attendee.BadgeID,
                        Name = co.Attendee.Name
                    }
                });
        }

        //POST api/checkouts/
        public async Task<IHttpActionResult> Post(PostCheckoutModel model)
        {
            var attendee = await _db.Attendees.FirstOrDefaultAsync(a => a.BadgeID == model.AttendeeBadgeNumber.Trim());
            var copyLibraryId = Convert.ToInt32(model.LibraryId.Replace("*", ""));
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
            var checkout = new Checkout { Attendee = attendee, Copy = copy, TimeOut = DateTime.Now };
            copy.CurrentCheckout = checkout;
            try
            {
                await _db.SaveChangesAsync();

                return Ok(checkout.ID);
            }
            catch
            {
                return BadRequest("Failed to save checkout");
            }
        }
    }
}
