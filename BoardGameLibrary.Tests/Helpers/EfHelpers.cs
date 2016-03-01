using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace BoardGameLibrary.Tests.Helpers
{
    public static class EfHelpers
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var observableCollection = new ObservableCollection<T>(sourceList);

            var dbSetMock = new Mock<DbSet<T>>();
            var mockEnumerable = new Mock<IDbAsyncEnumerable<T>>();
            var mockEnumerator = new Mock<IDbAsyncEnumerator<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSetMock.Setup(m => m.Local).Returns(observableCollection);
            dbSetMock.Setup(m => m.Include(It.IsAny<string>())).Returns(dbSetMock.Object);
            dbSetMock.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(mockEnumerator.Object);

            return dbSetMock;
        }

        public class MockedDbContext<T> : Mock<T> where T : DbContext
        {
            public Dictionary<string, object> Tables
            {
                get { return _Tables ?? (_Tables = new Dictionary<string, object>()); }
            }
            private Dictionary<string, object> _Tables;
        }

        /// <summary>
        /// Returns a mock of a DbContext
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MockedDbContext<T> GetMockContext<T>() where T : DbContext
        {
            var instance = new MockedDbContext<T>();
            instance.MockTables();
            return instance;
        }

        /// <summary>
        /// Use this method to mock a table, which is a DbSet{T} oject, in Entity Framework.
        /// Leave the second list null if no adds or deletes are used.
        /// </summary>
        /// <typeparam name="T">The table data type</typeparam>
        /// <param name="table">A List{T} that is being use to replace a database table.</param>
        /// <returns></returns>
        public static DbSet<T> MockDbSet<T>(List<T> table) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(q => q.Provider).Returns(() => table.AsQueryable().Provider);
            dbSet.As<IQueryable<T>>().Setup(q => q.Expression).Returns(() => table.AsQueryable().Expression);
            dbSet.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(() => table.AsQueryable().ElementType);
            dbSet.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(() => table.AsQueryable().GetEnumerator());
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>(table.Add);
            dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(table.AddRange);
            dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>(t => table.Remove(t));
            dbSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts =>
            {
                foreach (var t in ts) { table.Remove(t); }
            });
            return dbSet.Object;
        }

        /// <summary>
        /// Mocks all the DbSet{T} properties that represent tables in a DbContext.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mockedContext"></param>
        public static void MockTables<T>(this MockedDbContext<T> mockedContext) where T : DbContext
        {
            Type contextType = typeof(T);
            var dbSetProperties = contextType.GetProperties().Where(prop => (prop.PropertyType.IsGenericType) && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
            foreach (var prop in dbSetProperties)
            {
                var dbSetGenericType = prop.PropertyType.GetGenericArguments()[0];
                Type listType = typeof(List<>).MakeGenericType(dbSetGenericType);
                var listForFakeTable = Activator.CreateInstance(listType);
                var parameter = Expression.Parameter(contextType);
                var body = Expression.PropertyOrField(parameter, prop.Name);
                var lambdaExpression = Expression.Lambda<Func<T, object>>(body, parameter);
                var method = typeof(EfHelpers).GetMethod("MockDbSet").MakeGenericMethod(dbSetGenericType);
                mockedContext.Setup(lambdaExpression).Returns(method.Invoke(null, new[] { listForFakeTable }));
                mockedContext.Tables.Add(prop.Name, listForFakeTable);
            }
        }
    }
}
