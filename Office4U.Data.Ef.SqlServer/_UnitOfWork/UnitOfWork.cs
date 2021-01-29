using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.Users.Repositories;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using Office4U.WriteApplication.User.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            _context = context;
            ArticleRepository = new ArticleRepository(_context);
            UserRepository = new UserRepository(_context);
        }

        public IArticleRepository ArticleRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }


        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
