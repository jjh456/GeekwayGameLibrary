using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Api.Services;
using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Controllers
{
    [RoutePrefix("api/CopyCollections")]
    public class CopyCollectionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IFileUploadService uploadService;
        private readonly ICopiesRepository copiesRepository;

        public CopyCollectionsController()
        {
            copiesRepository = new CopiesRepository(db);
            uploadService = new FileUploadService(db, copiesRepository);
        }

        // GET: api/CopyCollections
        [ScopeAuthorize("read:game-collections")]
        public IEnumerable<CopyCollectionResponseModel> GetCopyCollections()
        {
            var copies = db.Copies.ToList();
            var collections = db.CopyCollections.ToList().Select(cc => new CopyCollectionResponseModel {
                ID = cc.ID,
                Name = cc.Name,
                Color = cc.Color,
                AllowWinning = cc.AllowWinning,
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
        public IHttpActionResult PutCopyCollection(int id, UpsertCopyCollectionModel copyCollection)
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
        public IHttpActionResult PostCopyCollection(UpsertCopyCollectionModel upsertCollectionModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(upsertCollectionModel.Name))
                return BadRequest("The name of the copy collection is required");
            var newCopyCollection = new CopyCollection {
                AllowWinning = upsertCollectionModel.DefaultWinnable,
                Name = upsertCollectionModel.Name,
                Color = upsertCollectionModel.Color
            };
            db.CopyCollections.Add(newCopyCollection);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = upsertCollectionModel.ID }, newCopyCollection);
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
        public IHttpActionResult PostCopy(int id, UpsertCopyRequestModel copyRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = db.CopyCollections.FirstOrDefault(cc => cc.ID == id);
            if (collection == null)
                return NotFound();

            var copyExistsAlready = db.Copies.Any(c => c.LibraryID == copyRequest.LibraryID);
            if (copyExistsAlready)
                return BadRequest("A copy with that ID exists already");

            copiesRepository.AddCopy(copyRequest.LibraryID, collection.ID, copyRequest.Title);

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [Route("{id}/copies/upload")]
        public IHttpActionResult UploadCopies(int id)
        {
            var files = HttpContext.Current.Request.Files;
            if (files == null || files.Count == 0)
                return BadRequest("You must provide a file");

            FileUploadResponse uploadResponse = uploadService.UploadCopiesFile(id, files[0]);

            return Ok(uploadResponse);
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