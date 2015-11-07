using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BoardGameLibrary.Models;
using BoardGameLibrary.Utility;
using System.Linq;
using PagedList;

namespace BoardGameLibrary.Views
{
    public class GamesController : Controller
    {
        private ApplicationDbContext _db;
        private FileUploader fileUploader;

        public GamesController()
        {
            _db = new ApplicationDbContext();
            fileUploader = new FileUploader(_db);
        }

        // GET: Games
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;
            int pageSize = 20;
            int pageNumber = page ?? 1;
            ViewBag.Errors = TempData["ErrorList"];

            var games = _db.Games.Select(g => g);
            if (!string.IsNullOrWhiteSpace(searchString))
                games = _db.Games.Where(g => g.Title.Contains(searchString));

            var model = new GameIndexViewModel { Games = games.OrderBy(g => g.Title).ToPagedList(pageNumber, pageSize) };

            return View(model);
        }

        // GET: Games/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Game game = await _db.Games.FindAsync(id);
            if (game == null)
                return HttpNotFound();

            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title")] Game game)
        {
            if (ModelState.IsValid)
            {
                _db.Games.Add(game);
                await _db.SaveChangesAsync();

                return RedirectToAction("Create", "Copies", new { gameID = game.ID });
            }

            return View(game);
        }

        [HttpPost]
        public ActionResult Import(GameIndexViewModel model)
        {
            IList<string> errors;
            var importFile = model.File;
            if (model == null || importFile == null || importFile.ContentLength == 0)
            {
                if (importFile == null)
                    errors = new List<string> { "The server didn't receive the file." };
                else
                    errors = new List<string> { "The file contents were empty." };
            }
            else
            {
                // Upload copies & games from the file.
                errors = fileUploader.UploadCopies(importFile);
            }

            TempData["ErrorList"] = new ErrorList { Errors = errors };
            return RedirectToAction("Index");
        }

        // GET: Games/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Game game = await _db.Games.FindAsync(id);
            if (game == null)
                return HttpNotFound();

            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title")] Game game)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(game).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await _db.Games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Game game = await _db.Games.FindAsync(id);
            _db.Games.Remove(game);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();

            base.Dispose(disposing);
        }
    }
}
