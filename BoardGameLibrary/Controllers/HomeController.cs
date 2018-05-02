using BoardGameLibrary.Data.Models;
using BoardGameLibrary.Models;
using System.Web.Mvc;
using System.Linq;

namespace BoardGameLibrary.Controllers
{
    public class HomeController : VolunteerController
    {
        private ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var numberOfPlays = _db.Plays.Count();
            var numberOfCheckouts = _db.Checkouts.Count();
            var model = new HomeModel
            {
                Statistics = new StatisticsModel
                {
                    NumberOfCheckouts = numberOfCheckouts,
                    NumberOfPlays = numberOfPlays
                }
            };
            return View(model);
        }
    }
}