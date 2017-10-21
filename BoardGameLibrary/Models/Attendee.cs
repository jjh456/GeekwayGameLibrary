using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoardGameLibrary.Models
{
    public class Attendee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Badge #")]
        public string BadgeID { get; set; }
        public virtual IList<Checkout> Checkouts { get; set; }
    }
}