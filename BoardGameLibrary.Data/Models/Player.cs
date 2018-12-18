using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameLibrary.Data.Models
{
    public class Player
    {
        public int ID { get; set; }
        public Attendee Attendee { get; set; }
        public virtual Rating Rating { get; set; }
        public bool WantsToWin { get; set; }

        [InverseProperty("Players")]
        public virtual Play Play { get; set; }

        public Player()
        {
            WantsToWin = true;
        }
    }
}