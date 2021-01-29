using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Article.Interfaces.IOC
{
    public interface IArticleRepository : IRepositoryBase
    {
        Task<Domain.Model.Entities.Articles.Article> GetArticleByIdAsync(int id);
    }
}