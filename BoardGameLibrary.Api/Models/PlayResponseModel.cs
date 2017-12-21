using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class PlayResponseModel
    {
        public int ID { get; set; }
        public int CheckoutID { get; set; }
        public int GameID { get; set; }
        public IEnumerable<PlayerResponseModel> Players { get; set; }
    }
}