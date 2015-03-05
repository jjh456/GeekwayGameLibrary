using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class AttendeeIndexViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public IList<Attendee> Attendees { get; set; }
    }
}