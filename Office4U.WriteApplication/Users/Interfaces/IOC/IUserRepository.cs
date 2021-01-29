using Office4U.Common;
using Office4U.Domain.Model.Users.Entities;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.User.Interfaces.IOC
{
    public interface IUserRepository : IRepositoryBase
    {
        Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
    }
}