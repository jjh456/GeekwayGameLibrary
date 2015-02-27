using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGameLibrary.Models
{
    public class Game
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual IList<Copy> Copies { get; set; }

        public Game()
        {
            Copies = new List<Copy>();
        }
    }
}