using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Controllers
{
    public class CopyCollectionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CopyCollections
        [ScopeAuthorize("read:game-collections")]
        public IQueryable<CopyCollection> GetCopyCollections()
        {
            return db.CopyCollections;
        }

        // GET: api/CopyCollections/5
        [ScopeAuthorize("read:game-collections")]
        [ResponseType(typeof(CopyCollection))]
        public IHttpActionResult GetCopyCollection(int id)
        {
            CopyCollection copyCollection = db.CopyCollections.Find(id);
            if (copyCollection == null)
                return NotFound();

            return Ok(copyCollection);
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
        public IHttpActionResult PostCopyCollection(CopyCollection copyCollection)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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