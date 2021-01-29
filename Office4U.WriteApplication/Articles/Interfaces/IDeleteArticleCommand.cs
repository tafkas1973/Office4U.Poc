using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Interfaces
{
    public interface IDeleteArticleCommand
    {
        Task<bool> Execute(int id);
    }
}
