using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.User.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Interfaces.IOC
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IArticleRepository ArticleRepository { get; }
        Task<bool> Commit();
        bool HasChanges();
    }
}