using Office4U.ReadApplication.Article.DTOs;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Article.Interfaces
{
    public interface IGetArticleQuery
    {
        Task<ArticleDto> Execute(int id);
    }
}
