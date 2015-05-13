using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class Checkout
    {
        public int ID { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime? TimeIn { get; set; }
        [NotMapped]
        [Display(Name = "Time Out")]
        public TimeSpan Length { 
            get 
            {
                if (TimeIn == null)
                    return DateTime.Now - TimeOut;
                else
                    return TimeIn.Value - TimeOut;
            } 
        }
        public virtual Attendee Attendee { get; set; }

        [InverseProperty("CheckoutHistory")]
        public virtual Copy Copy { get; set; }
    }
}