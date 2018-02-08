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

        public async Task<IHttpActionResult> Get(int id)
        {
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == id);

            return Ok(new CopyResponseModel(copy));
        }
    }
}