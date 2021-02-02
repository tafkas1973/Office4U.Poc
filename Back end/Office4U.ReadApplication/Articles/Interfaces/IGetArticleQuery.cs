using Office4U.ReadApplication.Articles.DTOs;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Interfaces
{
    public interface IGetArticleQuery
    {
        Task<ArticleDto> Execute(int id);
    }
}
