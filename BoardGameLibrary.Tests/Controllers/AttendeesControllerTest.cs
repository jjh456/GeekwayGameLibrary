using BoardGameLibrary.Controllers;
using BoardGameLibrary.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using BoardGameLibrary.Tests.Helpers;

namespace BoardGameLibrary.Tests.Controllers
{
    [TestFixture]
    public class AttendeesControllerTest
    {
        [Test]
        public async Task IndexQueriesAttendees()
        {
            var mockAttendeesDbSet = EfHelpers.GetQueryableMockDbSet(
                new Attendee { }
            );
            var db = new Mock<ApplicationDbContext>();
            db.Setup(d => d.Attendees)
              .Returns(mockAttendeesDbSet);
            var controller = new AttendeesController(db.Object);
            await controller.Index("", "", null);

            db.Verify(d => d.Attendees);
        }
    }
}
