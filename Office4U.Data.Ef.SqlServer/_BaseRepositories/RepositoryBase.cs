using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.WriteApplication.Interfaces.IOC;

namespace Office4U.Data.Ef.SqlServer.BaseRepositories
{
    public class RepositoryBase : IRepositoryBase
    {
        protected readonly DataContext _context;
        public RepositoryBase(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity)
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
    }
}