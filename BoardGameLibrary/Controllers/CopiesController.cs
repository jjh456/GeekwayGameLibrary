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
            ViewBag.GameTitle = game.Title;
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
        public async Task<ActionResult> Edit(Copy copy)
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

        public async Task<ActionResult> CheckOutCopy(CopyCheckOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var copyLibraryId = Convert.ToInt32(model.CopyLibraryID.Replace("*", ""));
                var copy = await db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
                var attendee = await db.Attendees.FirstOrDefaultAsync(a => a.BadgeID == model.AttendeeBadgeID.Replace("*", ""));
                var checkout = new Checkout { TimeOut = DateTime.Now, Attendee = attendee };
                copy.CurrentCheckout = checkout;

                await db.SaveChangesAsync();

                model.AttendeeBadgeID = "";
                model.CopyLibraryID = "";
                model.Messages.Add(string.Format("Copy {0} of {1} checked out to {2}({3}).", copyLibraryId, copy.Game.Title, attendee.Name, attendee.BadgeID));

                
            }
            else
                Response.StatusCode = 400;
            
            return PartialView("_CopyCheckOut", model);
        }

        public async Task<ActionResult> CheckInCopy(CopyCheckInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var copyLibraryId = Convert.ToInt32(model.CopyLibraryID.Replace("*", ""));
                var copy = await db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
                copy.CurrentCheckout.TimeIn = DateTime.Now;
                copy.CheckoutHistory.Add(copy.CurrentCheckout);
                copy.CurrentCheckout = null;

                await db.SaveChangesAsync();

                model.CopyLibraryID = "";
                model.Messages.Add(string.Format("Copy {0} of {1} checked in.", copy.LibraryID, copy.Game.Title));
            }
            else
                Response.StatusCode = 400;

            return PartialView("_CopyCheckIn", model);
        }

        public async Task<ActionResult> SearchCopies(CopySearchViewModel model)
        {
            if (ModelState.IsValid)
                return PartialView("_CopyList", await db.Copies.Where(c => c.Game.Title.Contains(model.GameTitle)).ToListAsync());
            else
                return GetModelStateErrorsJson();
        }

        private JsonResult GetModelStateErrorsJson()
        {
            var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            Response.StatusCode = 400;

            return Json(errorList);
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
