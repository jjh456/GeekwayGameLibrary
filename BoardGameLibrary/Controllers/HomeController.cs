using BoardGameLibrary.Models;
using System.Web.Mvc;

namespace BoardGameLibrary.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeModel();
            return View(model);
        }
    }
}