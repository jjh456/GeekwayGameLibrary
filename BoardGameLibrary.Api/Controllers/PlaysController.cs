using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Data.Models;
using BoardGameLibrary.Api.Models;
using System.Collections.Generic;

namespace BoardGameLibrary.Api.Controllers
{
    public class PlaysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Plays
        public IQueryable<Play> GetPlays()
        {
            return db.Plays;
        }

        // GET: api/Plays/5
        [ResponseType(typeof(Play))]
        public async Task<IHttpActionResult> GetPlay(int id)
        {
            Play play = await db.Plays.FindAsync(id);
            if (play == null)
            {
                return NotFound();
            }

            return Ok(play);
        }

        // PUT: api/Plays/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlay(int id, Play play)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != play.ID)
            {
                return BadRequest();
            }

            db.Entry(play).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Plays
        [ResponseType(typeof(Play))]
        public async Task<IHttpActionResult> PostPlay(PostPlayModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                players.Add(new Player { Attendee = attendee, Rating = rating, Play = play });
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
                if (PlayExists(play.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = play.ID }, play);
        }

        // DELETE: api/Plays/5
        [ResponseType(typeof(Play))]
        public async Task<IHttpActionResult> DeletePlay(int id)
        {
            Play play = await db.Plays.FindAsync(id);
            if (play == null)
            {
                return NotFound();
            }

            db.Plays.Remove(play);
            await db.SaveChangesAsync();

            return Ok(play);
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