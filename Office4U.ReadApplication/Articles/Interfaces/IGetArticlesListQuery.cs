using Office4U.Common;
using Office4U.ReadApplication.Article.DTOs;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Article.Interfaces
{
    public interface IGetArticlesListQuery
    {
        Task<PagedList<ArticleDto>> Execute(ArticleParams articleParams);
    }
}
