using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoardGameLibrary.Models
{
    public class Play
    {
        public int ID { get; set; }
        public virtual IList<Player> Players { get; set; }

        [Required]
        public virtual Checkout Checkout { get; set; }

        public Play()
        {
            Players = new List<Player>();
        }
    }
}