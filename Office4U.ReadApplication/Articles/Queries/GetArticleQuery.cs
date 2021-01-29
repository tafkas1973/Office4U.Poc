using AutoMapper;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Queries
{
    // TODO: implement MediatR ?

    public class GetArticleQuery : IGetArticleQuery
    {
        private readonly IReadOnlyArticleRepository _readOnlyArticleRepository;
        private readonly IMapper _mapper;

        public GetArticleQuery(
            IReadOnlyArticleRepository readOnlyArticleRepository, 
            IMapper mapper
            )
        {
            _readOnlyArticleRepository = readOnlyArticleRepository;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Execute(int id)
        {
            var article = await _readOnlyArticleRepository.GetArticleByIdAsync(id);

            var articleDto = _mapper.Map<ArticleDto>(article);

            return articleDto;
        }
    }
}
