using System.Collections.Generic;

namespace BoardGameLibrary.Data.Models
{
    public class CopyCollection
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<Copy> Copies { get; set; }

        public CopyCollection()
        {
            Copies = new List<Copy>();
        }
    }
}
