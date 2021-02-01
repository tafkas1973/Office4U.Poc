using AutoMapper;
using Office4U.Common;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using Office4U.ReadApplication.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Queries
{
    public class GetArticlesQuery : IGetArticlesQuery
    {
        private readonly IReadOnlyArticleRepository _readOnlyArticleRepository;

        public GetArticlesQuery(IReadOnlyArticleRepository articleRepository)
        {
            _readOnlyArticleRepository = articleRepository;
        }

        public async Task<PagedList<ArticleDto>> Execute(ArticleParams articleParams)
        {
            var articles = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

            // TODO: inject correct project AutoMapper
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            var articlesDtos = mapper.Map<IEnumerable<ArticleDto>>(articles);

            var articlesToReturn = new PagedList<ArticleDto>(articlesDtos, articles.TotalCount, articles.CurrentPage, articles.PageSize);

            return articlesToReturn;
        }
    }
}
