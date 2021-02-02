//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Office4U.Data.Ef.SqlServer.Contexts;
//using System;

//namespace Office4U.Data.Ef.SqlServer
//{
//    public class DatabaseFixture : IDisposable
//    {
//        public DataContext TestContext { get; private set; }
//        private readonly SqliteConnection _connection;
//        private readonly DbContextOptions _options;

//        public DatabaseFixture()
//        {
//            _connection = new SqliteConnection("datasource=:memory:");
//            _connection.Open();

//            _options = new DbContextOptionsBuilder()
//                .UseSqlite(_connection)
//                .Options;

//            TestContext = new DataContext(_options);
//            TestContext.Database.EnsureCreated();
//        }

//        public void Dispose()
//        {
//            // TestContext.Dispose();
//            _connection.Close();
//        }
//    }
//}
