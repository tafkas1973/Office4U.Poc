
using Office4U.Data.Ef.SqlServer.Contexts;

namespace Office4U.Data.Ef.SqlServer.BaseRepositories
{
    public class ReadOnlyRepositoryBase
    {
        protected readonly QueryDbContext _readOnlyContext;
        public ReadOnlyRepositoryBase(QueryDbContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }
    }
}