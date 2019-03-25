using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Controllers
{
    public class GameCollectionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GameCollections
        [ScopeAuthorize("read:game-collections")]
        public IQueryable<GameCollection> GetGameCollections()
        {
            return db.GameCollections;
        }

        // GET: api/GameCollections/5
        [ScopeAuthorize("read:game-collections")]
        [ResponseType(typeof(GameCollection))]
        public IHttpActionResult GetGameCollection(int id)
        {
            GameCollection gameCollection = db.GameCollections.Find(id);
            if (gameCollection == null)
                return NotFound();

            return Ok(gameCollection);
        }

        // PUT: api/GameCollections/5
        [ScopeAuthorize("update:game-collection")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGameCollection(int id, GameCollection gameCollection)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != gameCollection.ID)
                return BadRequest();

            db.Entry(gameCollection).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameCollectionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/GameCollections
        [ScopeAuthorize("create:game-collection")]
        [ResponseType(typeof(GameCollection))]
        public IHttpActionResult PostGameCollection(GameCollection gameCollection)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.GameCollections.Add(gameCollection);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gameCollection.ID }, gameCollection);
        }

        // DELETE: api/GameCollections/5
        [ScopeAuthorize("delete:game-collection")]
        [ResponseType(typeof(GameCollection))]
        public IHttpActionResult DeleteGameCollection(int id)
        {
            GameCollection gameCollection = db.GameCollections.Find(id);
            if (gameCollection == null)
                return NotFound();

            if (gameCollection.Games?.Count >= 0)
                return Conflict();

            db.GameCollections.Remove(gameCollection);
            db.SaveChanges();

            return Ok(gameCollection);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        private bool GameCollectionExists(int id)
        {
            return db.GameCollections.Count(e => e.ID == id) > 0;
        }
    }
}