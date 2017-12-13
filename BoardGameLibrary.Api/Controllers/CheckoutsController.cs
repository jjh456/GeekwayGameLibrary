using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

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
        public IEnumerable<Checkout> Get(string badgeId)
        {
            if (badgeId == null)
                return null;

            return _db.Checkouts
                .Where(co => co.Attendee.BadgeID == badgeId)
                .Where(co => co.Play == null)
                .ToList();
        }
    }
}
