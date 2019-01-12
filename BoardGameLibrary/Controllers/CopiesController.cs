using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BoardGameLibrary.Models;
using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Controllers
{
    public class CopiesController : VolunteerController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Copies
        public async Task<ActionResult> Index(int? gameID)
        {
            if (gameID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.GameID = gameID;

            var game = await _db.Games.FindAsync(gameID.Value);
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
            Copy copy = await _db.Copies.FindAsync(id);
            if (copy == null)
                return HttpNotFound();

            return View(copy);
        }

        // GET: Copies/Create
        public ActionResult Create(int? gameID)
        {
            if (gameID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var game = _db.Games.FirstOrDefault(g => g.ID == gameID.Value);
            var copy = new Copy { GameID = gameID.Value, Game = game };

            return View(copy);
        }

        // POST: Copies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Copy copy)
        {
            var preExistingCopy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copy.LibraryID);
            if (preExistingCopy != null)
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);

            var game = await _db.Games.FindAsync(copy.GameID);
            game.Copies.Add(copy);
            if (ModelState.IsValid)
            {
                _db.Copies.Add(copy);
                _db.Entry(game).State = EntityState.Modified;
                await _db.SaveChangesAsync();

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
            Copy copy = await _db.Copies.FindAsync(id);
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
                _db.Entry(copy).State = EntityState.Modified;
                await _db.SaveChangesAsync();
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
            Copy copy = await _db.Copies.FindAsync(id);
            if (copy == null)
                return HttpNotFound();

            return View(copy);
        }

        public async Task<ActionResult> CheckOutCopy(CopyCheckOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var copyLibraryId = Convert.ToInt32(model.CopyLibraryID.Replace("*", ""));
                var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
                var attendee = await _db.Attendees.FirstOrDefaultAsync(a => a.BadgeID == model.AttendeeBadgeID.Replace("*", ""));
                var checkout = new Checkout { TimeOut = DateTime.Now, Attendee = attendee, Copy = copy };
                copy.CurrentCheckout = checkout;

                await _db.SaveChangesAsync();

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
                var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
                copy.CurrentCheckout.TimeIn = DateTime.Now;
                copy.CheckoutHistory.Add(copy.CurrentCheckout);
                copy.CurrentCheckout = null;

                await _db.SaveChangesAsync();

                model.CopyLibraryID = "";
                model.Messages.Add(string.Format("Copy {0} of {1} checked in.", copy.LibraryID, copy.Game.Title));
            }
            else
                Response.StatusCode = 400;

            return PartialView("_CopyCheckIn", model);
        }

        [HttpPost]
        public async Task<JsonResult> CheckInCopyLight(string copyId)
        {
            if (ModelState.IsValid)
            {
                var copyLibraryId = Convert.ToInt32(copyId.Replace("*", ""));
                var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);
                copy.CurrentCheckout.TimeIn = DateTime.Now;
                copy.CheckoutHistory.Add(copy.CurrentCheckout);
                copy.CurrentCheckout = null;

                await _db.SaveChangesAsync();

                var msg = string.Format("Copy {0} of {1} checked in.", copy.LibraryID, copy.Game.Title);

                return Json(new { message = msg });
            }
            else
                return Json(new { message = "Failed to check in copy." });
        }

        public async Task<JsonResult> GetCopyGameTitle(string copyId)
        {
            if (ModelState.IsValid)
            {
                var copyLibraryId = Convert.ToInt32(copyId.Replace("*", ""));
                var copy = await _db.Copies.FirstOrDefaultAsync(c => c.LibraryID == copyLibraryId);

                if (copy == null)
                    return Json(new { title = "No copy found with that ID." }, JsonRequestBehavior.AllowGet);

                return Json(new { title = copy.Game.Title }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { title = "Copy ID required." }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SearchCopies(CopySearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                int searchedID;
                bool isNumeric = int.TryParse(model.Info, out searchedID);
                var allMatchedCopies = new List<Copy>();
                var copiesMatchingTitle = await _db.Copies.Where(c => c.Game.Title.Contains(model.Info)).ToListAsync();
                if (isNumeric)
                {
                    var copiesMatchingID = await _db.Copies.Where(c => c.LibraryID == searchedID).ToListAsync();
                    allMatchedCopies.AddRange(copiesMatchingID);
                }

                allMatchedCopies.AddRange(copiesMatchingTitle);
                allMatchedCopies = allMatchedCopies.Distinct().ToList();

                if (model.NavSearch)
                    return PartialView("_CopyNavSearchResults", allMatchedCopies);

                return PartialView("_CopyList", allMatchedCopies);
            }
            else
                return GetModelStateErrorsJson();
        }

        public ActionResult ListCheckedOutCopies()
        {
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderByDescending(c => c.CurrentCheckout.Length);

            return View(checkedOutCopies);
        }

        public ActionResult ListLongestCheckedOutCopies()
        {
            var checkedOutCopies = _db.Copies.Where(c => c.CurrentCheckout != null)
                                             .AsEnumerable()
                                             .OrderByDescending(c => c.CurrentCheckout.Length)
                                             .Take(10);

            return View("_LongestCheckedOutCopies", checkedOutCopies);
        }

        private JsonResult GetModelStateErrorsJson()
        {
            var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            Response.StatusCode = 400;

            return Json(errorList, JsonRequestBehavior.AllowGet);
        }

        // POST: Copies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Copy copy = await _db.Copies.FindAsync(id);
            var cgameID = copy.GameID;
            _db.Copies.Remove(copy);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", new { gameID = cgameID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();

            base.Dispose(disposing);
        }
    }
}
