using BoardGameLibrary.Data.Models;
using System;
using System.Linq;

namespace BoardGameLibrary.Api.Services
{
    public interface ICopiesRepository
    {
        void AddCopy(string copyLibraryId, int collectionId, string title, string ownerName = "");
    }

    public class CopiesRepository : ICopiesRepository
    {
        private readonly ApplicationDbContext _db;

        public CopiesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddCopy(string copyLibraryId, int collectionId, string title, string ownerName = "")
        {
            var collection = _db.CopyCollections.FirstOrDefault(cc => cc.ID == collectionId);

            var game = _db.Games.FirstOrDefault(g => g.Title == title);
            if (game == null)
            {
                game = new Game { Title = title };
                _db.Games.Add(game);
                try
                {
                    _db.SaveChanges();
                }
                catch(Exception e)
                {
                    throw new Exception($"Failed to add a game with the title {title}", e);
                }
            }
            var copy = new Copy
            {
                LibraryID = copyLibraryId,
                Game = game,
                GameID = game.ID,
                Winnable = collection.AllowWinning
            };

            collection.Copies.Add(copy);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add a copy with ID {copyLibraryId} and title {title}", e);
            }
        }
    }
}