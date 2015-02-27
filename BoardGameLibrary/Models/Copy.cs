using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class Copy
    {
        public int ID { get; set; }
        [ForeignKey("GameID")]
        public virtual  Game Game { get; set; }
        [Required]
        public int GameID { get; set; }
        [Required]
        public string OwnerName { get; set; }
        public virtual Checkout CurrentCheckout { get; set; }
        public virtual IList<Checkout> CheckoutHistory { get; set; }
        public string Notes { get; set; }

        public Copy()
        {
            CheckoutHistory = new List<Checkout>();
        }
    }
}