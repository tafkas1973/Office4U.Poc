using Office4U.WriteApplication.Article.DTOs;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Article.Interfaces
{
    public interface IUpdateArticleCommand
    {
        Task Execute(ArticleForUpdateDto article);
    }
}
