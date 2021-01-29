using Office4U.Common;
using Office4U.ReadApplication.Users.DTOs;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Users.Interfaces
{
    public interface IGetUsersQuery
    {
        Task<PagedList<AppUserDto>> Execute(UserParams userParams);
    }
}
