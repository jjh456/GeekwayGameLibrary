using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
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

        [ScopeAuthorize("read:copy-search")]
        public async Task<IHttpActionResult> Search(string query)
        {
            var copies = new List<Copy>();
            if (query != null)
            {
                copies = await _db.Copies.Where(
                    c => c.Game.Title.Contains(query) || c.LibraryID == query
                ).ToListAsync();
            }

            if (copies.Count == 0)
            {
                ModelState.AddModelError("query", "Couldn't find a copy that matched");
                return NotFound();
            }

            var copyResponseModels = copies.Select(c => new CopyResponseModel(c));
            if (copyResponseModels.Count() == 1)
                return Ok(copyResponseModels.First());

            return Ok(copyResponseModels);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == id);
            if (copy == null)
            {
                ModelState.AddModelError("id", "Couldn't find a copy with that ID");
                return NotFound();
            }

            return Ok(new CopyResponseModel(copy));
        }
    }
}