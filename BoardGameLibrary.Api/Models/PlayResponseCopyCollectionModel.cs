using BoardGameLibrary.Data.Models;
using System.Collections.Generic;

namespace BoardGameLibrary.Api.Models
{
    public class PlayResponseCopyCollectionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public PlayResponseCopyCollectionModel() {}

        public PlayResponseCopyCollectionModel(CopyCollection copyCollection, IEnumerable<CopyResponseModel> copies)
        {
            ID = copyCollection.ID;
            Name = copyCollection.Name;
        }
    }
}