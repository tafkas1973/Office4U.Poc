using System.Threading.Tasks;
using Office4U.Common;
using Office4U.Domain.Model.Entities.Users;
using Office4U.WriteApplication.Interfaces.IOC;

namespace Office4U.WriteApplication.User.Interfaces.IOC
{
    public interface IUserRepository: IRepositoryBase
    {
        Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);            
    }
}