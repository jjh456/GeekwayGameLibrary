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

        [HttpGet]
        [Route("api/checkouts/checkedOutLongest")]
        public async Task<IHttpActionResult> CheckedOutLongest(int numberOfResults = 10)
        {
            //var cop = _db.Copies.async
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderByDescending(c => c.CurrentCheckout.Length)
                                             .Take(numberOfResults)
                                             .Select(c => new CheckoutResponseModel(c.CurrentCheckout));

            return Ok(checkedOutCopies);
        }

        [HttpGet]
        [Route("api/checkouts/recentCheckouts")]
        public async Task<IHttpActionResult> RecentCheckouts(int numberOfResults = 5)
        {
            //var cop = _db.Copies.async
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderBy(c => c.CurrentCheckout.Length)
                                             .Take(numberOfResults)
                                             .Select(c => new CheckoutResponseModel(c.CurrentCheckout));

            return Ok(checkedOutCopies);
        }

        //GET api/checkouts/5 || api/checkouts? key = value
        public IEnumerable<CheckoutResponseModel> Get(string badgeId)
        {
            if (badgeId == null)
                return null;

            return _db.Checkouts
                .Where(co => co.Attendee.BadgeID == badgeId && co.Play == null)
                .Select(co => new CheckoutResponseModel(co));
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

                return Ok(new CheckoutResponseModel(checkout));
            }
            catch
            {
                return BadRequest("Failed to save checkout");
            }
        }

        //PUT api/checkouts/checkin
        [HttpPut]
        [Route("api/checkouts/checkin/{copyId}")]
        public async Task<IHttpActionResult> CheckIn(int copyId)
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
