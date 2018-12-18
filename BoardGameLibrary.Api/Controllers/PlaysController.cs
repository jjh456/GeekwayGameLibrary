using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Data.Models;
using BoardGameLibrary.Api.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BoardGameLibrary.Api.Controllers
{
    public class PlaysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Plays
        [ScopeAuthorize("read:plays")]
        public GetPlaysResponse GetPlays()
        {
            var playsResponse = new GetPlaysResponse();
            playsResponse.Plays = db.Plays
                .Select(play => new PlayResponseModel
                {
                    ID = play.ID,
                    CheckoutID = play.Checkout.ID,
                    GameID = play.Checkout.Copy.GameID,
                    GameName = play.Checkout.Copy.Game.Title,
                    Players = play.Players.Select(player => new PlayerResponseModel {
                        ID = player.Attendee.BadgeID,
                        Name = player.Attendee.Name,
                        WantsToWin = player.WantsToWin
                    })
                })
                .ToList();

            return playsResponse;
        }

        // GET: api/Plays/5
        [ResponseType(typeof(Play))]
        [ScopeAuthorize("read:plays")]
        public async Task<IHttpActionResult> GetPlay(int id)
        {
            Play play = await db.Plays.FindAsync(id);
            if (play == null)
            {
                return NotFound();
            }

            return Ok(play);
        }

        // GET: api/Plays/5
        //[ResponseType(typeof(IList<Play>))]
        //[ScopeAuthorize("create:checkout")]
        //[System.Web.Http.Route("api/plays/GeneratePlays")]
        //[System.Web.Http.HttpPost]
        //public IHttpActionResult GeneratePlayData([FromUri] int numberOfPlays)
        //{
        //    var generator = new PlayGenerator(db);
        //    try
        //    {
        //        var plays = generator.GeneratePlays(numberOfPlays);
        //        var numberOfPlaysGenerated = plays.Count;

        //        return Ok(string.Format("{0} Plays generated successfully", numberOfPlaysGenerated));
        //    }
        //    catch(Exception e)
        //    {
        //        return InternalServerError(e);
        //    }
        //}

        // POST: api/Plays
        [ResponseType(typeof(Play))]
        public async Task<HttpStatusCodeResult> PostPlay(PostPlayModel request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var checkout = db.Checkouts.FirstOrDefault(c => c.ID == request.CheckoutId);
            var game = checkout.Copy.Game;
            var playerAttendeeIds = request.Players.Select(p => p.Id).ToList();
            var play = new Play { Checkout = checkout };
            var players = new List<Player>();
            foreach (var requestPlayer in request.Players)
            {
                var attendee = db.Attendees.FirstOrDefault(a => a.ID == requestPlayer.Id);
                var rating = new Rating { Value = requestPlayer.Rating };
                game.Ratings.Add(rating);
                players.Add(new Player { Attendee = attendee, Rating = rating, Play = play, WantsToWin = requestPlayer.WantsToWin });
            }
            play.Players = players;

            db.Plays.Add(play);
            db.Players.AddRange(players);
            checkout.Play = play;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayExists(int id)
        {
            return db.Plays.Count(e => e.ID == id) > 0;
        }
    }
}