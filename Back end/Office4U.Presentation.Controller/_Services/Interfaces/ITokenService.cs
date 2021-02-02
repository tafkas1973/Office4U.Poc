using Office4U.Domain.Model.Users.Entities;
using System.Threading.Tasks;

namespace Office4U.Presentation.Controller.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
