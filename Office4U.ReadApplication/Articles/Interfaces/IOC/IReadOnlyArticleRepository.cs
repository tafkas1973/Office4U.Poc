using Office4U.Common;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.ReadApplication.Interfaces;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Interfaces.IOC
{
    public interface IReadOnlyArticleRepository : IReadOnlyRepositoryBase
    {
        Task<PagedList<Article>> GetArticlesAsync(ArticleParams articleParams);
        Task<Article> GetArticleByIdAsync(int id);
    }
}