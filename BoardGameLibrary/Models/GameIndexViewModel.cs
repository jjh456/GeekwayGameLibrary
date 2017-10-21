using BoardGameLibrary.Data.Models;
using System.Web;
using PagedList;

namespace BoardGameLibrary.Models
{
    public class GameIndexViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public IPagedList<Game> Games { get; set; }
    }
}