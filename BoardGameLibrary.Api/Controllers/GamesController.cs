using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BoardGameLibrary.Api.Controllers
{
    public class GamesController : ApiController
    {
        private ApplicationDbContext db;

        public GamesController()
        {
            db = new ApplicationDbContext();
        }

        // GET: api/Games
        public GetGamesResponseModel Get()
        {
            var gamesResponse = new GetGamesResponseModel();
            gamesResponse.Games = db.Games
                .Select(game => new GameResponseModel
                {
                    ID = game.ID,
                    Name = game.Title,
                    Copies = game.Copies.Select(copy => new CopyResponseModel { id = copy.ID })
                })
                .ToList();

            return gamesResponse;
        }
    }
}
