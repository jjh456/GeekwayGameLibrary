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
        public IEnumerable<Checkout> Get(string attendeeBadgeId)
        {
            if (attendeeBadgeId == null)
                return null;

            return _db.Checkouts
                .Where(co => co.Attendee.BadgeID == attendeeBadgeId)
                .ToList();
        }

        //POST api/checkouts
        public void Post([FromBody]string value)
        {
        }

        // PUT api/checkouts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/checkouts/5
        public void Delete(int id)
        {
        }
    }
}
