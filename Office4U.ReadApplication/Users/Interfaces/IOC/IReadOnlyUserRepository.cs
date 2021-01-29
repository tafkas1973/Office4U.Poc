using Office4U.Common;
using Office4U.Domain.Model.Entities.Users;
using Office4U.ReadApplication.Interfaces;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Users.Interfaces.IOC
{
    public interface IReadOnlyUserRepository : IReadOnlyRepositoryBase
    {
        Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByNameAsync(string name);
    }
}