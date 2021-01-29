using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Interfaces
{
    public interface IDeleteArticleCommand
    {
        Task Execute(int id);
    }
}
