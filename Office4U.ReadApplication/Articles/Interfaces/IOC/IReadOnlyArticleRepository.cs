using Office4U.Common;
using Office4U.ReadApplication.Interfaces;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Article.Interfaces.IOC
{
    public interface IReadOnlyArticleRepository : IReadOnlyRepositoryBase
    {
        Task<PagedList<Domain.Model.Entities.Articles.Article>> GetArticlesAsync(ArticleParams articleParams);
        Task<Domain.Model.Entities.Articles.Article> GetArticleByIdAsync(int id);
    }
}