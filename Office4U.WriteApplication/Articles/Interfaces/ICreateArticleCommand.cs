using Office4U.WriteApplication.Article.DTOs;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Article.Interfaces
{
    public interface ICreateArticleCommand
    {
        Task Execute(ArticleForCreationDto articleForCreation);
    }
}
