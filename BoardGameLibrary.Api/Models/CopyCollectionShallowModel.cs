using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Models
{
    public class CopyCollectionShallowModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool AllowWinning { get; set; }
        public string Color { get; set; }

        public CopyCollectionShallowModel() {}

        public CopyCollectionShallowModel(CopyCollection copyCollection)
        {
            ID = copyCollection.ID;
            Name = copyCollection.Name;
            AllowWinning = copyCollection.AllowWinning;
            Color = copyCollection.Color;
        }
    }
}