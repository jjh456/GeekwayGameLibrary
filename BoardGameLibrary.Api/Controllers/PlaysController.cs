﻿using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BoardGameLibrary.Data.Models;
using BoardGameLibrary.Api.Models;
using System.Collections.Generic;
using NLog;
using Newtonsoft.Json;
using System;
using System.Data.Entity;

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
            logger.Debug("Get plays called.");
            db.Database.CommandTimeout = 120;
            var playsResponse = new GetPlaysResponse();
            try
            {
                db.Database.Log = logger.Debug;

                logger.Debug("Retrieving all of the players.");
                var allPlayers = db.Players
                    .Include(p => p.Play)
                    .Include(p => p.Rating)
                    .Select(player => new PlayerResponseModel
                {
                    ID = player.Attendee.BadgeID,
                    Name = player.Attendee.Name,
                    WantsToWin = player.WantsToWin,
                    Rating = player.Rating.Value,
                    PlayID = player.Play.ID
                });
                logger.Debug("Retrieved all of the players. Next we query plays.");
                var playsQuery = db.Plays
                    .Include(play => play.Checkout)
                    .Select(play => new PlayResponseModel
                    {
                        ID = play.ID,
                        CheckoutID = play.Checkout.ID,
                        GameID = play.Checkout.Copy.GameID,
                        GameName = play.Checkout.Copy.Game.Title,
                        Players = allPlayers.Where(p => p.PlayID == play.ID).ToList(),
                        Collection = new CopyCollectionShallowModel
                        {
                            ID = play.Checkout.Copy.CopyCollection.ID,
                            Name = play.Checkout.Copy.CopyCollection.Name
                        },
                        Checkout = new PlayResponseCheckoutModel
                        {
                            ID = play.Checkout.ID,
                            TimeIn = play.Checkout.TimeIn,
                            TimeOut = play.Checkout.TimeOut
                        }
                    });

                playsResponse.Plays = playsQuery.ToList();

                logger.Debug($"Retrieved {playsResponse.Plays.Count} plays.");

                return playsResponse;
            }
            catch(Exception e)
            {
                logger.Error(e, "An error occurred while retrieving plays");
                throw;
            }
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
                logger.Error(e, "An error occurred while saving a play.");
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