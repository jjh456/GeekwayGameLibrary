using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Controllers
{
    [RoutePrefix("api/CopyCollections")]
    public class CopyCollectionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CopyCollections
        [ScopeAuthorize("read:game-collections")]
        public IEnumerable<CopyCollectionResponseModel> GetCopyCollections()
        {
            var copies = db.Copies.ToList();
            var collections = db.CopyCollections.ToList().Select(cc => new CopyCollectionResponseModel {
                Copies = copies.Where(copy => copy.CopyCollectionID == cc.ID).Select(copy => new CopyResponseModel(copy)).ToList()
            });

            return collections;
        }

        // GET: api/CopyCollections/5
        [ScopeAuthorize("read:game-collections")]
        [ResponseType(typeof(CopyCollectionResponseModel))]
        public IHttpActionResult GetCopyCollection(int id)
        {
            CopyCollection copyCollection = db.CopyCollections.Find(id);

            if (copyCollection == null)
                return NotFound();

            var copies = db.Copies.Where(c => c.CopyCollectionID == id).ToList().Select(c => new CopyResponseModel(c));
            CopyCollectionResponseModel response = new CopyCollectionResponseModel(copyCollection, copies);

            return Ok(response);
        }

        // PUT: api/CopyCollections/5
        [ScopeAuthorize("update:game-collection")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCopyCollection(int id, CopyCollection copyCollection)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != copyCollection.ID)
                return BadRequest();

            db.Entry(copyCollection).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CopyCollectionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/CopyCollections
        [ScopeAuthorize("create:game-collection")]
        [ResponseType(typeof(CopyCollection))]
        [HttpPost]
        public IHttpActionResult PostCopyCollection(CopyCollection copyCollection)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(copyCollection.Name))
                return BadRequest("The name of the copy collection is required");

            db.CopyCollections.Add(copyCollection);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = copyCollection.ID }, copyCollection);
        }

        // DELETE: api/CopyCollections/5
        [ScopeAuthorize("delete:game-collection")]
        [ResponseType(typeof(CopyCollection))]
        public IHttpActionResult DeleteCopyCollection(int id)
        {
            CopyCollection copyCollection = db.CopyCollections.Find(id);
            if (copyCollection == null)
                return NotFound();

            if (copyCollection.Copies?.Count > 0)
                return Conflict();

            db.CopyCollections.Remove(copyCollection);
            db.SaveChanges();

            return Ok(copyCollection);
        }

        // POST: api/CopyCollections/{collectionId}/
        [ScopeAuthorize("create:game-collection")]
        [ResponseType(typeof(CopyCollection))]
        [HttpPost()]
        [Route("{id}/copies")]
        public IHttpActionResult PostCopy(int id, CreateCopyRequestModel copyRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = db.CopyCollections.FirstOrDefault(cc => cc.ID == id);
            if (collection == null)
                return NotFound();

            var copyExistsAlready = db.Copies.Any(c => c.LibraryID == copyRequest.LibraryID);
            if (copyExistsAlready)
                return BadRequest("A copy with that ID exists already");

            var game = db.Games.FirstOrDefault(g => g.Title == copyRequest.Title);
            if (game == null)
            {
                game = new Game { Title = copyRequest.Title };
                db.Games.Add(game);
                db.SaveChanges();
            }
            var copy = new Copy { LibraryID = copyRequest.LibraryID, Game = game, GameID = game.ID };

            collection.Copies.Add(copy);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.Created);
        }

        //[ScopeAuthorize("read:copies")]
        [ScopeAuthorize("read:game-collection")]
        [ResponseType(typeof(IList<CopyResponseModel>))]
        [HttpGet]
        [Route("{id}/copies")]
        public IHttpActionResult GetCopies(int id)
        {
            CopyCollection copyCollection = db.CopyCollections.Find(id);
            if (copyCollection == null)
                return NotFound();

            var copies = copyCollection.Copies.Select(c => new CopyResponseModel(c));

            return Ok(copies);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        private bool CopyCollectionExists(int id)
        {
            return db.CopyCollections.Count(e => e.ID == id) > 0;
        }
    }
}