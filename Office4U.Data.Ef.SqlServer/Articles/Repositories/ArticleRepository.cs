using Microsoft.EntityFrameworkCore;
using Office4U.Data.Ef.SqlServer.BaseRepositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Articles.Repositories
{
    public class ArticleRepository : RepositoryBase, IArticleRepository
    {
        public ArticleRepository(DataContext context) : base(context) { }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await Context.Articles
                .Include(a => a.Photos)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}