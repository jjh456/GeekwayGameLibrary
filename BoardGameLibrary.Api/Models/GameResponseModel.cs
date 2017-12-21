using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class GameResponseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<CopyResponseModel> Copies { get; set; }
    }
}