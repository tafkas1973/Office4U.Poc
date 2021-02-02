using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Tests.TestData;
using System;

namespace Office4U.Data.Ef.SqlServer
{
    public class DatabaseFixture : IDisposable
    {
        public CommandDbContext TestContext { get; private set; }
        public QueryDbContext TestReadOnlyContext { get; private set; }

        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<CommandDbContext> _options;
        private readonly DbContextOptions<QueryDbContext> _readOnlyOptions;

        public DatabaseFixture()
        {
            _connection = new SqliteConnection("datasource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<CommandDbContext>()
                .UseSqlite(_connection)
                .Options;
            TestContext = new CommandDbContext(_options);
            TestContext.Database.EnsureCreated();
            TestContext.Articles.AddRange(ArticleList.GetDefaultList());
            TestContext.SaveChanges();

            _readOnlyOptions = new DbContextOptionsBuilder<QueryDbContext>()
                .UseSqlite(_connection)
                .Options;
            TestReadOnlyContext = new QueryDbContext(_readOnlyOptions);
            TestReadOnlyContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
