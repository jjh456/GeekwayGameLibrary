using BoardGameLibrary.Data.Models;
using System.Web;
using PagedList;

namespace BoardGameLibrary.Models
{
    public class AttendeeIndexViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public IPagedList<Attendee> Attendees { get; set; }
    }
}