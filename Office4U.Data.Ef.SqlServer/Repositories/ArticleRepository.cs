using Microsoft.EntityFrameworkCore;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Entities.Articles;
using Office4U.WriteApplication.Article.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Repositories
{
    public class ArticleRepository : RepositoryBase, IArticleRepository
    {
        public ArticleRepository(DataContext context) : base(context) { }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Photos)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
    }
}