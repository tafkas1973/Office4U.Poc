using Office4U.WriteApplication.Articles.DTOs;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Interfaces
{
    public interface ICreateArticleCommand
    {
        Task<ArticleForReturnDto> Execute(ArticleForCreationDto articleForCreation);
    }
}
