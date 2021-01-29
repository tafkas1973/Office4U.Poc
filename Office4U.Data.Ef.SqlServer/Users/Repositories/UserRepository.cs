using Microsoft.EntityFrameworkCore;
using Office4U.Common;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.Repositories;
using Office4U.Domain.Model.Users.Entities;
using Office4U.WriteApplication.User.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Users.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams)
        {
            var users = _context.Users
                .AsQueryable();

            return await PagedList<AppUser>.CreateAsync(
                users,
                userParams.PageNumber,
                userParams.PageSize);
        }
    }
}