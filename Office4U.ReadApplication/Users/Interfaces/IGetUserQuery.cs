using Office4U.ReadApplication.Users.DTOs;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Users.Interfaces
{
    public interface IGetUserQuery
    {
        Task<AppUserDto> Execute(int id);
        Task<AppUserDto> Execute(string name);
    }
}
