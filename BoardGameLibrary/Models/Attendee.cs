using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class Attendee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Badge #")]
        public string BadgeID { get; set; }
    }
}