using BoardGameLibrary.Data.Models;

namespace BoardGameLibrary.Api.Models
{
    public class UpsertCopyCollectionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool DefaultWinnable { get; set; }

        public UpsertCopyCollectionModel()
        {
            DefaultWinnable = false;
            Color = CopyCollection.GetRandomColor();
        }
    }
}