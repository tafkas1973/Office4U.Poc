using Office4U.WriteApplication.Articles.DTOs;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Interfaces
{
    public interface ICreateArticleCommand
    {
        Task Execute(ArticleForCreationDto articleForCreation);
    }
}
