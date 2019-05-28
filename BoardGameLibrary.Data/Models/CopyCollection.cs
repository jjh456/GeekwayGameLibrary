using System;
using System.Collections.Generic;

namespace BoardGameLibrary.Data.Models
{
    public class CopyCollection
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool AllowWinning { get; set; }
        public IList<Copy> Copies { get; set; }

        public CopyCollection()
        {
            Copies = new List<Copy>();
            Color = GetRandomColor();
        }

        public static string GetRandomColor()
        {
            var random = new Random();
            return string.Format("#{0:X6}", random.Next(0x1000000)); 
        }
    }
}
