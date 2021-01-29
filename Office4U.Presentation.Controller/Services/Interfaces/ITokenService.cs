using System.Threading.Tasks;
using Office4U.Domain.Model.Entities.Users;

namespace Office4U.Presentation.Controller.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
