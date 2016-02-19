using BoardGameLibrary.Models;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace BoardGameLibrary.Tests.Helpers
{
    public static class EfHelpers
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            var mockEnumerable = new Mock<IDbAsyncEnumerable<T>>();
            var mockEnumerator = new Mock<IDbAsyncEnumerator<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(mockEnumerator.Object);

            return dbSet.Object;
        }
    }
}
