using AutoMapper;
using Office4U.Common;
using Office4U.ReadApplication.Article.DTOs;
using Office4U.ReadApplication.Article.Interfaces;
using Office4U.ReadApplication.Article.Interfaces.IOC;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Article.Queries
{
    // TODO: implement MediatR ?

    public class GetArticlesListQuery : IGetArticlesListQuery
    {
        private readonly IReadOnlyArticleRepository _readOnlyArticleRepository;
        private readonly IMapper _mapper;

        public GetArticlesListQuery(IReadOnlyArticleRepository articleRepository, IMapper mapper)
        {
            _readOnlyArticleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<ArticleDto>> Execute(ArticleParams articleParams)
        {
            var articles = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

            // TODO: find AutoMapper solution iso projection
            //var articlesDtos = _mapper.Map<IEnumerable<ArticleDto>>(articles);
            var articlesDtos = articles.Select(a => new ArticleDto()
            {
                Id = a.Id,
                Code = a.Code,
                Name1 = a.Name1,
                SupplierId = a.SupplierId,
                SupplierReference = a.SupplierReference,
                PurchasePrice = a.PurchasePrice,
                Unit = a.Unit,
                Photos = a.Photos.Select(p => new ArticlePhotoDto() { Id = p.Id, IsMain = p.IsMain, Url = p.Url }).ToList(),
                PhotoUrl = a.Photos.Any() ? a.Photos.First().Url : string.Empty
            });


            var articlesToReturn = new PagedList<ArticleDto>(articlesDtos, articles.TotalCount, articles.CurrentPage, articles.PageSize);

            return articlesToReturn;
        }
    }
}
