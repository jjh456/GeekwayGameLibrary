using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameLibrary.Models
{
    public class Checkout
    {
        public int ID { get; set; }
        [Display(Name = "Checked out at")]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime TimeOut { get; set; }
        [Display(Name = "Checked in at")]
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTime? TimeIn { get; set; }
        [NotMapped]
        [Display(Name = "Checkout length")]
        [DisplayFormat(DataFormatString = "{0:%d} days {0:%h} hours {0:%m} minutes", ApplyFormatInEditMode = true)]
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

        public virtual Play Play { get; set; }
    }
}