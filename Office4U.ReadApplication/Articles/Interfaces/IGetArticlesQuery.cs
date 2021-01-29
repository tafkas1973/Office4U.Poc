using Office4U.Common;
using Office4U.ReadApplication.Articles.DTOs;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Interfaces
{
    public interface IGetArticlesQuery
    {
        Task<PagedList<ArticleDto>> Execute(ArticleParams articleParams);
    }
}
