using System.Threading.Tasks;

namespace Office4U.WriteApplication.Article.Interfaces
{
    public interface IDeleteArticleCommand
    {
        Task Execute(int id);
    }
}
