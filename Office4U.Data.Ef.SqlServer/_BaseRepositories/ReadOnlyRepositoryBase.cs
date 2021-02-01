
using Office4U.Data.Ef.SqlServer.Contexts;

namespace Office4U.Data.Ef.SqlServer.BaseRepositories
{
    public class ReadOnlyRepositoryBase
    {
        protected readonly ReadOnlyDataContext _readOnlyContext;
        public ReadOnlyRepositoryBase(ReadOnlyDataContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }
    }
}