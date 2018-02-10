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

        public async Task<IHttpActionResult> Get(string query)
        {
            var copies = new List<Copy>();
            if (query != null)
            {
                int searchedID = -1;
                bool isNumeric = false;
                try
                {
                    isNumeric = int.TryParse(query, out searchedID);
                }
                catch
                {
                    System.Console.WriteLine("Couldn't parse int");
                }
                var copiesMatchingTitle = await _db.Copies.Where(c => c.Game.Title.Contains(query)).ToListAsync();
                var copiesMatchingID = new List<Copy>();
                if (isNumeric && searchedID != -1)
                {
                    copiesMatchingID = await _db.Copies.Where(c => c.LibraryID == searchedID).ToListAsync();
                    copies.AddRange(copiesMatchingID);
                }

                copies.AddRange(copiesMatchingTitle);
                copies = copies.Distinct().ToList();
            }
            var copyResponseModels = copies.Select(c => new CopyResponseModel(c));

            return Ok(copyResponseModels);
        }

        public async Task<IHttpActionResult> Get(int id)
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