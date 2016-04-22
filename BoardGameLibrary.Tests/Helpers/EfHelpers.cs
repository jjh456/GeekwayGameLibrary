using Moq;
using System.Data.Entity;
using System.Linq;

namespace BoardGameLibrary.Tests.Helpers
{
    public static class EfHelpers
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSetMock;
        }

        public static Mock<DbSet<T>> GetEmptyQueryableMockDbSet<T>() where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();

            return dbSetMock;
        }
    }
}
