using BoardGameLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BoardGameLibrary.Api.Controllers
{
    public class PlayGenerator
    {
        private readonly ApplicationDbContext db;
        private IList<Copy> allWinnableCopies;
        private IList<Attendee> allAttendees;
        private Random random;

        public PlayGenerator(ApplicationDbContext db)
        {
            this.db = db;
            random = new Random();
            allWinnableCopies = db.Copies.Where(c => c.CopyCollection.AllowWinning).ToList();
            allAttendees = db.Attendees.ToList();
        }

        public IList<Play> GeneratePlays(int numberOfPlays)
        {
            var plays = new List<Play>();
            var checkouts = GenerateCheckouts(numberOfPlays);
            foreach (var checkout in checkouts)
            {
                var play = new Play { Checkout = checkout };
                play.Players = GeneratePlayers(play);
                checkout.Play = play;
            }
            db.Plays.AddRange(plays);
            db.SaveChanges();

            return plays;
        }

        private IList<Checkout> GenerateCheckouts(int numberOfCheckouts)
        {
            var checkouts = new List<Checkout>();
            for (int i = 0; i < numberOfCheckouts; i++)
                checkouts.Add(GenerateCheckout());

            db.Checkouts.AddRange(checkouts);
            db.SaveChanges();

            return checkouts;
        }

        private Checkout GenerateCheckout()
        {
            Attendee attendee = GetRandomAttendee();

            var copyIndex = random.Next(0, allWinnableCopies.Count - 1);
            var copy = allWinnableCopies.ElementAtOrDefault(copyIndex);

            var checkout = new Checkout { Attendee = attendee, Copy = copy, TimeOut = DateTime.Now, TimeIn = DateTime.Now.AddSeconds(10) };

            return checkout;
        }

        private IList<Player> GeneratePlayers(Play play)
        {
            var players = new List<Player>();
            var numberOfPlayers = random.Next(1, 7);
            for (int i = 0; i < numberOfPlayers; i++)
                players.Add(new Player { Attendee = GetRandomAttendee(), Play = play });

            return players.Distinct().ToList();
        }

        private Attendee GetRandomAttendee()
        {
            var index = random.Next(0, allAttendees.Count - 1);
            var attendee = allAttendees.ElementAtOrDefault(index);
            return attendee;
        }
    }
}