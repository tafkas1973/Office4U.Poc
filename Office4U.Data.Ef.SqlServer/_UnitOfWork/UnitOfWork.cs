using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using Office4U.WriteApplication.User.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CommandDbContext _context;

        public UnitOfWork(
            CommandDbContext context,
            IArticleRepository articleRepository,
            IUserRepository userRepository)
        {
            _context = context;
            ArticleRepository = articleRepository;
            UserRepository = userRepository;
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
