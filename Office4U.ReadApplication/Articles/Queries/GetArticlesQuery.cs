using AutoMapper;
using Office4U.Common;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Queries
{
    public class GetArticlesQuery : IGetArticlesQuery
    {
        private readonly IReadOnlyArticleRepository _readOnlyArticleRepository;
        private readonly IMapper _mapper;

        public GetArticlesQuery(
            IReadOnlyArticleRepository articleRepository,
            IMapper mapper
            )
        {
            _readOnlyArticleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<ArticleDto>> Execute(ArticleParams articleParams)
        {
            var articles = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);
            var articlesDtos = _mapper.Map<IEnumerable<ArticleDto>>(articles);

            var articlesToReturn = new PagedList<ArticleDto>(articlesDtos, articles.TotalCount, articles.CurrentPage, articles.PageSize);

            return articlesToReturn;
        }
    }
}
