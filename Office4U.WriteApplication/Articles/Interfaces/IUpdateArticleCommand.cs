using Office4U.WriteApplication.Articles.DTOs;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Interfaces
{
    public interface IUpdateArticleCommand
    {
        Task Execute(ArticleForUpdateDto article);
    }
}
