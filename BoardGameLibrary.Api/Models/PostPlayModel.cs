using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class PostPlayModel
    {
        public IList<PostPlayerModel> Players { get; set; }
        public int CheckoutId { get; set; }
    }
}