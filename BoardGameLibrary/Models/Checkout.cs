using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class Checkout
    {
        public int ID { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime TimeIn { get; set; }
        public virtual Patron Patron { get; set; }
    }
}