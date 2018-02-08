using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BoardGameLibrary.Api.Controllers
{
    public class CopiesController : ApiController
    {
        private ApplicationDbContext _db;

        public CopiesController()
        {
            _db = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("api/copies/checkedOutLongest")]
        public async Task<IHttpActionResult> CheckedOutLongest(int numberOfResults = 10)
        {
            //var cop = _db.Copies.async
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderByDescending(c => c.CurrentCheckout.Length)
                                             .Take(numberOfResults)
                                             .Select(c => new CopyResponseModel(c));

            return Ok(checkedOutCopies);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == id);

            return Ok(new CopyResponseModel(copy));
        }
    }
}