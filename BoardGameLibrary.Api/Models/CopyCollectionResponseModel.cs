using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace BoardGameLibrary.Api.Models
{
    public class CopyCollectionResponseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<CopyResponseModel> Copies { get; set; }

        public CopyCollectionResponseModel()
        {
            Copies = new List<CopyResponseModel>();
        }

        public CopyCollectionResponseModel(CopyCollection copyCollection, IEnumerable<CopyResponseModel> copies)
        {
            ID = copyCollection.ID;
            Name = copyCollection.Name;
            Copies = copies.ToList();
        }
    }
}