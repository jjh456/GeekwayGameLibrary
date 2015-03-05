using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class GameIndexViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public IList<Game> Games { get; set; }
    }
}