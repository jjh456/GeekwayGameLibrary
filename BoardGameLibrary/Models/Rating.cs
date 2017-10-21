using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameLibrary.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public int? Value { get; set; }

        [Required]
        public virtual Player Player { get; set; }

        [InverseProperty("Ratings")]
        public Game Game {
            get {
                return Player.Play.Checkout.Copy.Game;
            }
        }
    }
}