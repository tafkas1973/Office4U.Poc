using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Interfaces.IOC
{
    public interface IArticleRepository : IRepositoryBase
    {
        Task<Article> GetArticleByIdAsync(int id);
    }
}