using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Data.Models;
using BoardGameLibrary.Api.Models;
using System.Collections.Generic;
using NLog;
using Newtonsoft.Json;

namespace BoardGameLibrary.Api.Controllers
{
    public class PlaysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Plays
        [ScopeAuthorize("read:plays")]
        public GetPlaysResponse GetPlays()
        {
            var playsResponse = new GetPlaysResponse();
            var checkouts = db.Plays.Select(p => p.Checkout);
            var collections = db.CopyCollections;
            playsResponse.Plays = db.Plays
                .Select(play => new PlayResponseModel
                {
                    ID = play.ID,
                    CheckoutID = play.Checkout.ID,
                    Collection = new CopyCollectionShallowModel
                    {
                        ID = play.Checkout.Copy.CopyCollection.ID,
                        Name = play.Checkout.Copy.CopyCollection.Name
                    },
                    GameID = play.Checkout.Copy.GameID,
                    GameName = play.Checkout.Copy.Game.Title,
                    Checkout = new PlayResponseCheckoutModel {
                        ID = play.Checkout.ID,
                        TimeIn = play.Checkout.TimeIn,
                        TimeOut = play.Checkout.TimeOut
                    },
                    Players = play.Players.Select(player => new PlayerResponseModel
                    {
                        ID = player.Attendee.BadgeID,
                        Name = player.Attendee.Name,
                        WantsToWin = player.WantsToWin
                    })
                })
                .ToList();

            return playsResponse;
        }

        // GET: Plays/5
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

        //POST: Plays/5
        //[ResponseType(typeof(IList<Play>))]
        //[ScopeAuthorize("create:checkout")]
        //[System.Web.Http.Route("plays/GeneratePlays")]
        //[System.Web.Http.HttpPost]
        //public IHttpActionResult GeneratePlayData([FromUri] int numberOfPlays)
        //{
        //    var numberOfPlaysBefore = db.Plays.Count();
        //    var generator = new PlayGenerator(db);
        //    try
        //    {
        //        var plays = generator.GeneratePlays(numberOfPlays);
        //        var numberOfPlaysAfter = db.Plays.Count();
        //        var numberOfPlaysGenerated = numberOfPlaysAfter - numberOfPlaysBefore;

        //        return Ok(string.Format($"{numberOfPlaysGenerated} Plays have been generated"));
        //    }
        //    catch (Exception e)
        //    {
        //        return InternalServerError(e);
        //    }
        //}

        // POST: Plays
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostPlay(PostPlayModel request)
        {
            logger.Debug($"Posting a play. It has the following shape: {JsonConvert.SerializeObject(request)}");
            if (!ModelState.IsValid || request == null)
                return BadRequest();

            var checkout = db.Checkouts.FirstOrDefault(c => c.ID == request.CheckoutId);
            logger.Debug("Retrieved play's checkout.");

            if (checkout.Play != null)
                return BadRequest("A play has already been entered for that checkout!");

            if (!checkout.Copy.CopyCollection.AllowWinning)
                return BadRequest("Winning prizes is not allowed for that checkout.");

            var game = checkout.Copy.Game;
            var playerAttendeeIds = request.Players.Select(p => p.Id).ToList();
            var play = new Play { Checkout = checkout };
            var players = new List<Player>();
            foreach (var requestPlayer in request.Players)
            {
                var attendee = db.Attendees.FirstOrDefault(a => a.ID == requestPlayer.Id);
                logger.Debug($"Looked up attendee with badge ID {attendee.BadgeID} and name {attendee.Name}");

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
            catch (DbUpdateException e)
            {
                logger.Error("An error occurred while saving a play.", e);
                throw;
            }

            return Ok(play.ID);
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