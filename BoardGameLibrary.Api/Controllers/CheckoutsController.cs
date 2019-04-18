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

        //GET checkouts
        public IEnumerable<Checkout> Get() => _db.Checkouts.ToList();

        [HttpGet]
        [Route("checkouts/checkedOutLongest")]
        [ScopeAuthorize("read:longest-checkouts")]
        public async Task<IHttpActionResult> CheckedOutLongest()
        {
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderByDescending(c => c.CurrentCheckout.Length)
                                             .Select(c => new CheckoutResponseModel(c.CurrentCheckout));

            return Ok(checkedOutCopies);
        }

        [HttpGet]
        [Route("checkouts/recentCheckouts")]
        [ScopeAuthorize("read:recent-checkouts")]
        public async Task<IHttpActionResult> RecentCheckouts(int numberOfResults = 5)
        {
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderBy(c => c.CurrentCheckout.Length)
                                             .Take(numberOfResults)
                                             .Select(c => new CheckoutResponseModel(c.CurrentCheckout));

            return Ok(checkedOutCopies);
        }

        //GET checkouts/5 || checkouts? key = value
        public IEnumerable<CheckoutResponseModel> Get(string badgeId)
        {
            if (String.IsNullOrWhiteSpace(badgeId))
                return null;
            
            var matchingAttendees = _db.Attendees.Where(a => a.BadgeID.Equals(badgeId));
            if (matchingAttendees.Count() > 1)
            {
                return null;
            }

            var attendee = matchingAttendees.FirstOrDefault();

            return attendee.Checkouts
                .Where(co => co.Play == null)
                .ToList()
                .Select(co => new CheckoutResponseModel(co, true));
        }

        //POST checkouts/
        [ScopeAuthorize("create:checkout")]
        public async Task<IHttpActionResult> Post(PostCheckoutModel model)
        {
            var attendee = await _db.Attendees.FirstOrDefaultAsync(a => a.BadgeID == model.AttendeeBadgeNumber.Trim());
            if (attendee == null)
                return BadRequest("Attendee not found");

            var copyLibraryId = model.LibraryId.Replace("*", "");
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
            if (copy == null)
                return BadRequest("Copy not found");

            var checkout = new Checkout { Attendee = attendee, Copy = copy, TimeOut = DateTime.Now };
            copy.CurrentCheckout = checkout;
            try
            {
                await _db.SaveChangesAsync();

                return Ok(new CheckoutResponseModel(checkout));
            }
            catch
            {
                return BadRequest("Failed to save checkout");
            }
        }

        //PUT checkouts/checkin
        [HttpPut]
        [Route("checkouts/checkin/{copyId}")]
        [ScopeAuthorize("update:checkout")]
        public async Task<IHttpActionResult> CheckIn(string copyId)
        {
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyId);
            if (copy == null)
            {
                ModelState.AddModelError("id", "Failed to look up the copy");
                return NotFound();
            }
            if (copy.CurrentCheckout == null)
            {
                ModelState.AddModelError("id", "That copy is not checked out");
                return NotFound();
            }
            copy.CurrentCheckout.TimeIn = DateTime.Now;
            copy.CheckoutHistory.Add(copy.CurrentCheckout);
            var copyCheckingIn = copy.CurrentCheckout;
            copy.CurrentCheckout = null;
            try
            {
                await _db.SaveChangesAsync();

                return Ok(new CheckoutResponseModel(copyCheckingIn));
            }
            catch
            {
                ModelState.AddModelError("id", "Failed to look up the checkout");
                return BadRequest("Failed to save checkout");
            }
        }
    }
}
