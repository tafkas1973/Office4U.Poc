using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.WriteApplication.Interfaces.IOC;

namespace Office4U.Data.Ef.SqlServer.BaseRepositories
{
    public class RepositoryBase : IRepositoryBase
    {
        protected readonly DataContext Context;
        public RepositoryBase(DataContext context)
        {
            Context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            Context.Add(entity);
        }

        public void Update<T>(T entity)
        {
            Context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Remove(entity);
        }
    }
}