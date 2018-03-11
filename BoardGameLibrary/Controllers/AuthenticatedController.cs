using System.Web.Mvc;

namespace BoardGameLibrary.Controllers
{
    [Authorize(Roles = "AdminUI-Admin")]
    public class AdminController : Controller {}

    [Authorize(Roles = "AdminUI-Admin,AdminUI-Volunteer")]
    public class VolunteerController : Controller {}
}