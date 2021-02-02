using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Tests.TestData;
using System;

namespace Office4U.Data.Ef.SqlServer
{
    public class DatabaseFixture : IDisposable
    {
        public DataContext TestContext { get; private set; }
        public ReadOnlyDataContext TestReadOnlyContext { get; private set; }

        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<DataContext> _options;
        private readonly DbContextOptions<ReadOnlyDataContext> _readOnlyOptions;

        public DatabaseFixture()
        {
            _connection = new SqliteConnection("datasource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;
            TestContext = new DataContext(_options);
            TestContext.Database.EnsureCreated();
            TestContext.Articles.AddRange(ArticleList.GetDefaultList());
            TestContext.SaveChanges();

            _readOnlyOptions = new DbContextOptionsBuilder<ReadOnlyDataContext>()
                .UseSqlite(_connection)
                .Options;           
            TestReadOnlyContext = new ReadOnlyDataContext(_readOnlyOptions);
            TestReadOnlyContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
