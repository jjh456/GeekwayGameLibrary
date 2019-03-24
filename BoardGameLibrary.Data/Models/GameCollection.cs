using System.Collections.Generic;

namespace BoardGameLibrary.Data.Models
{
    public class GameCollection
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<Game> Games { get; set; }
    }
}
