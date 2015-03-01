using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BoardGameLibrary.Models;

namespace BoardGameLibrary.Controllers
{
    public class CopiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Copies
        public async Task<ActionResult> Index(int? gameID)
        {
            if (gameID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.GameID = gameID;

            var game = await db.Games.FindAsync(gameID.Value);
            var copies = game.Copies;

            return View(copies);
        }

        // GET: Copies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Copy copy = await db.Copies.FindAsync(id);
            if (copy == null)
                return HttpNotFound();

            return View(copy);
        }

        // GET: Copies/Create
        public ActionResult Create(int? gameID)
        {
            if (gameID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var copy = new Copy { GameID = gameID.Value };

            return View(copy);
        }

        // POST: Copies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Copy copy)
        {
            var game = await db.Games.FindAsync(copy.GameID);
            game.Copies.Add(copy);
            if (ModelState.IsValid)
            {
                db.Copies.Add(copy);
                db.Entry(game).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { gameID = copy.GameID });
            }

            return View(copy);
        }

        // GET: Copies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Copy copy = await db.Copies.FindAsync(id);
            if (copy == null)
            {
                return HttpNotFound();
            }
            return View(copy);
        }

        // POST: Copies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,OwnerName,Notes")] Copy copy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(copy).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { gameID = copy.GameID });
            }
            return View(copy);
        }

        // GET: Copies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Copy copy = await db.Copies.FindAsync(id);
            if (copy == null)
                return HttpNotFound();

            return View(copy);
        }

        public async Task<ActionResult> CheckoutCopy(string copyLibraryID, string attendeeBadgeID)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> CheckinCopy(string copyLibraryID)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> SearchCopies(string copyLibraryID)
        {
            throw new NotImplementedException();
        }

        // POST: Copies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Copy copy = await db.Copies.FindAsync(id);
            var cgameID = copy.GameID;
            db.Copies.Remove(copy);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { gameID = cgameID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
