using Microsoft.EntityFrameworkCore;
using Office4U.Common;
using Office4U.Data.Ef.SqlServer.BaseRepositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Articles.Repositories
{
    public class ReadOnlyArticleRepository : ReadOnlyRepositoryBase, IReadOnlyArticleRepository
    {
        public ReadOnlyArticleRepository(ReadOnlyDataContext readOnlyContext) : base(readOnlyContext) { }

        public async Task<PagedList<Article>> GetArticlesAsync(
            ArticleParams articleParams)
        {
            IQueryable<Article> query = _readOnlyContext.Articles
                .Include(a => a.Photos)
                .AsQueryable();

            query = FilterQuery(articleParams, query);
            query = OrderByQuery(articleParams, query);

            return await PagedList<Article>.CreateAsync(
                query,
                articleParams.PageNumber,
                articleParams.PageSize);
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _readOnlyContext.Articles
                .Include(a => a.Photos)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        private static IQueryable<Article> FilterQuery(
            ArticleParams articleParams,
            IQueryable<Article> articles)
        {
            if (!string.IsNullOrEmpty(articleParams.Code))
            {
                articles = articles.Where(a => a.Code.ToUpper().Contains(articleParams.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(articleParams.SupplierId))
            {
                articles = articles.Where(a => a.SupplierId.ToUpper().Contains(articleParams.SupplierId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(articleParams.SupplierReference))
            {
                articles = articles.Where(a => a.SupplierReference.ToUpper().Contains(articleParams.SupplierReference.ToUpper()));
            }

            if (!string.IsNullOrEmpty(articleParams.Name1))
            {
                articles = articles.Where(a => a.Name1.ToUpper().Contains(articleParams.Name1.ToUpper()));
            }

            if (!string.IsNullOrEmpty(articleParams.Unit))
            {
                articles = articles.Where(a => a.Unit.ToUpper().Contains(articleParams.Unit.ToUpper()));
            }

            if (articleParams.PurchasePriceMin != null)
            {
                articles = articles.Where(a => a.PurchasePrice >= articleParams.PurchasePriceMin);
            }

            if (articleParams.PurchasePriceMax != null)
            {
                articles = articles.Where(a => a.PurchasePrice <= articleParams.PurchasePriceMax);
            }

            return articles;
        }

        private static IQueryable<Article> OrderByQuery(
            ArticleParams articleParams,
            IQueryable<Article> articles)
        {
            // TODO: use enums instead of magic strings
            articles = articleParams.OrderBy switch
            {
                "code" => articles.OrderBy(a => a.Code),
                "supplierReference" => articles.OrderBy(a => a.SupplierReference),
                "name" => articles.OrderBy(a => a.Name1),
                _ => articles.OrderBy(a => a.Code)
            };

            return articles;
        }
    }
}