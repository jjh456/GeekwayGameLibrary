using BoardGameLibrary.Controllers;
using BoardGameLibrary.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using BoardGameLibrary.Tests.Helpers;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BoardGameLibrary.Utility;

namespace BoardGameLibrary.Tests.Controllers
{
    [TestFixture]
    public class AttendeesControllerTest
    {
        Mock<DbSet<Attendee>> mockAttendeesDbSet;
        ApplicationDbContext fakeDb = new ApplicationDbContext();
        AppSettings appSettings = new AppSettings();

        string joseName = "Jose Vladivoskov";
        string klausName = "Klaus Gruber";

        [SetUp]
        public void SetUp()
        {
            mockAttendeesDbSet = EfHelpers.GetQueryableMockDbSet(
                new Attendee { Name = klausName, BadgeID = "22", ID = 1 },
                new Attendee { Name = joseName, BadgeID = "111", ID = 2 },
                new Attendee { Name = "Not who you are looking for", BadgeID = "0", ID = 3 }
            );
            
            fakeDb.Attendees = mockAttendeesDbSet.Object;
        }

        public void SetUpEmptyAttendeesDb()
        {
            mockAttendeesDbSet = EfHelpers.GetEmptyQueryableMockDbSet<Attendee>();

            fakeDb.Attendees = mockAttendeesDbSet.Object;
        }

        [Test]
        public async Task IndexQueriesAttendees()
        {
            var controller = new AttendeesController(fakeDb, appSettings);

            var viewResult = (ViewResult)await controller.Index("", "", null);
            var modelResult = (AttendeeIndexViewModel)viewResult.Model;

            Assert.That(modelResult.Attendees.Count, Is.EqualTo(mockAttendeesDbSet.Object.Count())); 
        }

        [Test]
        public async Task IndexAlphabetizesAttendeesByName()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb, appSettings);

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
            var controller = new AttendeesController(fakeDb, appSettings);

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
            var controller = new AttendeesController(fakeDb, appSettings);

            // Act
            var viewResult = (ViewResult)await controller.Index("", "1", null);
            var modelResult = (AttendeeIndexViewModel)viewResult.Model;

            Assert.That(modelResult.Attendees.Count, Is.EqualTo(1));
            Assert.That(modelResult.Attendees.Any(m => m.Name == joseName));
        }

        [Test]
        public async Task DetailsReturnsHttpNotFound()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb, appSettings);

            // Act
            var viewResult = await controller.Details(4);

            Assert.That(viewResult.GetType, Is.EqualTo(typeof(HttpNotFoundResult)));
        }

        [Test]
        public async Task DetailsNullChecksId()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb, appSettings);

            // Act
            var viewResult = (HttpStatusCodeResult)await controller.Details(null);

            Assert.AreEqual(viewResult.StatusCode, 400);
        }

        [Test]
        public async Task DetailsReturnsMatchedAttendee()
        {
            // Arrange
            var controller = new AttendeesController(fakeDb, appSettings);

            // Act
            var viewResult = (ViewResult)await controller.Details(1);
            var modelResult = (Attendee)viewResult.Model;

            Assert.That(modelResult.ID, Is.EqualTo(1));
        }
    }
}
