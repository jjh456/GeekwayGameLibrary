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
        [HttpGet]
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

            return Ok(copyResponseModels);
        }

        [ScopeAuthorize("read:copy")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == id);
            var trimmedId = id.TrimStart('0');
            if (copy == null)
                copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == trimmedId);

            if (copy == null)
            {
                ModelState.AddModelError("id", "Couldn't find a copy with that ID");
                return NotFound();
            }

            return Ok(new CopyResponseModel(copy));
        }

        [HttpPut]
        [ScopeAuthorize("update:copy")]
        public async Task<IHttpActionResult> Put(string id, UpsertCopyRequestModel copyRequest)
        {
            var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == id);
            if (copy == null)
            {
                ModelState.AddModelError("id", "Couldn't find a copy with that ID");
                return NotFound();
            }

            if (copyRequest.LibraryID.ToLower() != id.ToLower())
                copy.LibraryID = copyRequest.LibraryID;

            var originalGame = copy.Game;
            if (copyRequest.Title != copy.Game.Title)
            {
                var newGame = _db.Games.FirstOrDefault(g => g.Title == copyRequest.Title);
                if (newGame == null)
                {
                    newGame = new Game { Title = copyRequest.Title };
                    _db.Games.Add(newGame);
                    _db.SaveChanges();
                }
                copy.Game = newGame;
                copy.GameID = newGame.ID;
            }
            if (copyRequest.CollectionID.HasValue)
            {
                var newCollection = _db.CopyCollections.FirstOrDefault(cc => cc.ID == copyRequest.CollectionID.Value);
                if (newCollection == null)
                    return BadRequest("The chosen collection does not seem to exist.");

                if (newCollection.ID != copy.CopyCollectionID)
                {
                    copy.CopyCollection = newCollection;
                    copy.CopyCollectionID = newCollection.ID;
                }
            }
            if (copyRequest.Winnable.HasValue)
                copy.Winnable = copyRequest.Winnable.Value;

            await _db.SaveChangesAsync();

            return Ok(new CopyResponseModel(copy));
        }
    }
}