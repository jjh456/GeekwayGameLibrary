using BoardGameLibrary.Controllers;
using BoardGameLibrary.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using BoardGameLibrary.Tests.Helpers;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BoardGameLibrary.Tests.Controllers
{
    [TestFixture]
    public class AttendeesControllerTest
    {
        Mock<DbSet<Attendee>> mockAttendeesDbSet;
        ApplicationDbContext fakeDb = new ApplicationDbContext();

        string joseName = "Jose Vladivoskov";
        string klausName = "Klaus Gruber";

        [SetUp]
        public void SetUp()
        {
            mockAttendeesDbSet = EfHelpers.GetQueryableMockDbSet(
                new Attendee { Name = klausName, BadgeID = "22" },
                new Attendee { Name = joseName, BadgeID = "111" },
                new Attendee { Name = "Not who you are looking for", BadgeID = "0" }
            );
            
            fakeDb.Attendees = mockAttendeesDbSet.Object;
        }

        [Test]
        public async Task IndexQueriesAttendees()
        {
            var controller = new AttendeesController(fakeDb);

            var viewResult = (ViewResult)await controller.Index("", "", null);
            var modelResult = (AttendeeIndexViewModel)viewResult.Model;

            Assert.That(modelResult.Attendees.Count, Is.EqualTo(mockAttendeesDbSet.Object.Count())); 
        }

        [Test]
        public async Task IndexAlphabetizesAttendeesByName()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb);

            // Act
            var viewResult = (ViewResult)await controller.Index("", "", null);
            var modelResult = (AttendeeIndexViewModel)viewResult.Model;

            Assert.That(modelResult.Attendees[0].Name, Is.EqualTo(joseName));
            Assert.That(modelResult.Attendees[1].Name, Is.EqualTo(klausName));
        }

        [Test]
        public async Task IndexFiltersAttendeesByNameUsingSearchString()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb);

            // Act
            var viewResult = (ViewResult)await controller.Index("", "Jos", null);
            var modelResult = (AttendeeIndexViewModel)viewResult.Model;

            Assert.That(modelResult.Attendees.Count, Is.EqualTo(1));
            Assert.That(modelResult.Attendees.Any(m => m.Name == joseName));
        }

        [Test]
        public async Task IndexFiltersAttendeesByBadgeIdUsingSearchString()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb);

            // Act
            var viewResult = (ViewResult)await controller.Index("", "1", null);
            var modelResult = (AttendeeIndexViewModel)viewResult.Model;

            Assert.That(modelResult.Attendees.Count, Is.EqualTo(1));
            Assert.That(modelResult.Attendees.Any(m => m.Name == joseName));
        }
    }
}
