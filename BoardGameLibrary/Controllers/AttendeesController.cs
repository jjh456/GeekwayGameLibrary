using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BoardGameLibrary.Models;
using BoardGameLibrary.Utility;
using PagedList;

namespace BoardGameLibrary.Controllers
{
    public class AttendeesController : Controller
    {
        private ApplicationDbContext _db;
        private FileUploader fileUploader;

        public AttendeesController()
        {
            _db = new ApplicationDbContext();
            fileUploader = new FileUploader(_db);
        }

        public AttendeesController(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        // GET: Attendees
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;
            int pageSize = 15;
            int pageNumber = page ?? 1;
            ViewBag.Errors = TempData["ErrorList"];

            IQueryable<Attendee> attendees = _db.Attendees;

            if (!string.IsNullOrWhiteSpace(searchString))
                attendees = attendees.Where(a => a.Name.Contains(searchString) || a.BadgeID.Contains(searchString));

            var model = new AttendeeIndexViewModel();
            var orderedAttendees = attendees.OrderBy(a => a.Name).ToList();
            model.Attendees = orderedAttendees.ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: Attendees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Attendee Attendee = _db.Attendees.FirstOrDefault(a => a.ID == id.Value);
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
                _db.Attendees.Add(Attendee);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(Attendee);
        }

        // GET: Attendees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Attendee Attendee = await _db.Attendees.FindAsync(id);
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
                _db.Entry(Attendee).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Attendee);
        }

        [HttpPost]
        public ActionResult Import(AttendeeIndexViewModel model)
        {
            IList<string> errors;
            var importFile = model.File;
            if (importFile == null || importFile == null || importFile.ContentLength == 0)
            {
                if (importFile == null)
                    errors = new List<string> { "The server didn't receive the file." };
                else
                    errors = new List<string> { "The file contents were empty." };
            }
            else
            {
                // Upload attendees from the file.
                errors = fileUploader.UploadAttendees(importFile);
            }

            TempData["ErrorList"] = new ErrorList { Errors = errors };
            return RedirectToAction("Index");
        }

        // GET: Attendees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Attendee Attendee = await _db.Attendees.FindAsync(id);
            if (Attendee == null)
                return HttpNotFound();

            return View(Attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Attendee Attendee = await _db.Attendees.FindAsync(id);
            _db.Attendees.Remove(Attendee);
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
