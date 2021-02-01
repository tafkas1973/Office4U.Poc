using Microsoft.EntityFrameworkCore;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Tests.TestData;
using System;

namespace Office4U.Data.Ef.SqlServer
{
    public class DatabaseFixture : IDisposable
    {
        public DataContext TestContext { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(DateTime.Now.ToString("yyyyMMddHHmmss") + "_TestDatabase")
                //.UseSqlite(new SqliteConnection("DataSource=:memory:")).Options;
                .Options;

            TestContext = new DataContext(options);
            TestContext.Articles.AddRange(ArticleList.GetDefaultList());
            TestContext.SaveChanges();
        }

        public void Dispose()
        {
            TestContext.Dispose();
        }
    }
}
