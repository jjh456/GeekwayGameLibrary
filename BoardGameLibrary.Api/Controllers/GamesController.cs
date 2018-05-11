using BoardGameLibrary.Api.Models;
using BoardGameLibrary.Data.Models;
using System;
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
        [ScopeAuthorize("read:games")]
        public GetGamesResponseModel Get()
        {
            try
            {
                var gamesResponse = new GetGamesResponseModel();
                gamesResponse.Games = db.Games.ToList()
                    .Select(game => new GameResponseModel
                    {
                        ID = game.ID,
                        Name = game.Title,
                        Copies = game.Copies.Select(copy => new CopyResponseModel(copy))
                    })
                    .ToList();

                return gamesResponse;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("api", e.Message);
                throw;
            }
        }
    }
}
