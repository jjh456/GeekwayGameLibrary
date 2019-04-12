using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Models
{
    public class CopyCollectionShallowModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public CopyCollectionShallowModel() {}

        public CopyCollectionShallowModel(CopyCollection copyCollection)
        {
            ID = copyCollection.ID;
            Name = copyCollection.Name;
        }
    }
}