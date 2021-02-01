using Microsoft.EntityFrameworkCore;
using Office4U.Common;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.BaseRepositories;
using Office4U.Domain.Model.Users.Entities;
using Office4U.ReadApplication.Users.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Users.Repositories
{
    public class ReadOnlyUserRepository : ReadOnlyRepositoryBase, IReadOnlyUserRepository
    {
        public ReadOnlyUserRepository(ReadOnlyDataContext readOnlyContext) : base(readOnlyContext) { }

        public async Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams)
        {
            var query = _readOnlyContext.Users.AsQueryable();

            return await PagedList<AppUser>.CreateAsync(
                query,
                userParams.PageNumber,
                userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _readOnlyContext.Users
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            return await _readOnlyContext.Users
                .SingleOrDefaultAsync(u => u.UserName == name);
        }
    }
}