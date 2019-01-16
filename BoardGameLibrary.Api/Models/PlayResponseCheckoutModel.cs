using System;

namespace BoardGameLibrary.Api.Models
{
    public class PlayResponseCheckoutModel
    {
        public int ID { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime? TimeIn { get; set; }
    }
}