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
    public class AttendeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attendees
        public async Task<ActionResult> Index()
        {
            return View(await db.Attendees.ToListAsync());
        }

        // GET: Attendees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Attendee Attendee = await db.Attendees.FindAsync(id);
            if (Attendee == null)
                return HttpNotFound();

            return View(Attendee);
        }

        // GET: Attendees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attendees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,BadgeID")] Attendee Attendee)
        {
            if (ModelState.IsValid)
            {
                db.Attendees.Add(Attendee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(Attendee);
        }

        // GET: Attendees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Attendee Attendee = await db.Attendees.FindAsync(id);
            if (Attendee == null)
                return HttpNotFound();

            return View(Attendee);
        }

        // POST: Attendees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,BadgeID")] Attendee Attendee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Attendee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Attendee);
        }

        // GET: Attendees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Attendee Attendee = await db.Attendees.FindAsync(id);
            if (Attendee == null)
                return HttpNotFound();

            return View(Attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Attendee Attendee = await db.Attendees.FindAsync(id);
            db.Attendees.Remove(Attendee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
