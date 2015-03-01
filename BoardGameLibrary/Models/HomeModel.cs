using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class HomeModel
    {
        public CopyCheckOutViewModel CheckOut { get; set; }
        public CopyCheckInViewModel CheckIn { get; set; }
        public CopySearchViewModel CopySearch { get; set; }

        public HomeModel()
        {
            CheckIn = new CopyCheckInViewModel();
            CheckOut = new CopyCheckOutViewModel();
            CopySearch = new CopySearchViewModel();
        }
    }
}