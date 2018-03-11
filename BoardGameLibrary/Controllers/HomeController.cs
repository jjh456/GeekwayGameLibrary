using BoardGameLibrary.Models;
using System.Web.Mvc;

namespace BoardGameLibrary.Controllers
{
    public class HomeController : VolunteerController
    {
        public ActionResult Index()
        {
            var model = new HomeModel();
            return View(model);
        }
    }
}