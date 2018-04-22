using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BoardGameLibrary.Data.Models;
using PagedList;
using BoardGameLibrary.Utility;

namespace BoardGameLibrary.Controllers
{
    public class CheckoutsController : VolunteerController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AppSettings _appSettings = new AppSettings();

        const string timeOutDescSort = "time_out_desc";
        const string timeOutAscSort = "time_out_asc";
        const string playDescSort = "play_desc";
        const string playAscSort = "play_asc";

        // GET: Checkouts
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page, string sortOrder = timeOutDescSort)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.TimeOutSort = sortOrder == timeOutDescSort ? timeOutAscSort : timeOutDescSort;
            ViewBag.PlaySort = sortOrder == playDescSort ? playAscSort : playDescSort;
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            int pageSize = _appSettings.PageSize;
            int pageNumber = page ?? 1;

            var results = db.Checkouts.Select(c => c);

            if (!string.IsNullOrWhiteSpace(searchString))
                results = results.Where(c => c.Copy.Game.Title.Contains(searchString)
                                          || c.Copy.LibraryID.ToString().Contains(searchString)
                                          || c.Attendee.Name.Contains(searchString)
                                          || c.Attendee.BadgeID.Contains(searchString));

            switch (sortOrder)
            {
                case playDescSort:
                    results = results.OrderByDescending(c => c.Play == null);
                    break;
                case playAscSort:
                    results = results.OrderBy(c => c.Play == null);
                    break;
                case timeOutDescSort:
                    results = results.OrderByDescending(c => c.TimeOut);
                    break;
                default:
                    results = results.OrderBy(c => c.TimeOut);
                    break;
            }

            return View(results.ToPagedList(pageNumber, pageSize));
        }

        // GET: Checkouts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = await db.Checkouts.FindAsync(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // GET: Checkouts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Checkouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,TimeOut,TimeIn")] Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Checkouts.Add(checkout);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(checkout);
        }

        // GET: Checkouts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = await db.Checkouts.FindAsync(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TimeOut,TimeIn")] Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkout).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(checkout);
        }

        // GET: Checkouts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = await db.Checkouts.FindAsync(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Checkout checkout = await db.Checkouts.FindAsync(id);
            db.Checkouts.Remove(checkout);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
