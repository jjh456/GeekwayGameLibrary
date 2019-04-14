using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace BoardGameLibrary.Api.Models
{
    public class CopyCollectionResponseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool AllowWinning { get; set; }
        public string Color { get; set; }
        public IList<CopyResponseModel> Copies { get; set; }

        public CopyCollectionResponseModel()
        {
            Copies = new List<CopyResponseModel>();
        }

        public CopyCollectionResponseModel(CopyCollection copyCollection, IEnumerable<CopyResponseModel> copies)
        {
            ID = copyCollection.ID;
            Name = copyCollection.Name;
            AllowWinning = copyCollection.AllowWinning;
            Color = copyCollection.Color;
            Copies = copies.ToList();
        }
    }
}