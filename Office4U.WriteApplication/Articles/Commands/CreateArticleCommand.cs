using AutoMapper;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Helpers;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.Articles.WriteApplication.Article.Commands
{
    public class CreateArticleCommand : ICreateArticleCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateArticleCommand(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ArticleForReturnDto> Execute(ArticleForCreationDto articleForCreation)
        {
            // TODO: inject correct project AutoMapper
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            var newArticle = mapper.Map<Domain.Model.Articles.Entities.Article>(articleForCreation);

            _unitOfWork.ArticleRepository.Add(newArticle);

            if (await _unitOfWork.Commit())
            {
                var articleForReturnDto = _mapper.Map<ArticleForReturnDto>(newArticle);                
                return articleForReturnDto;
            }

            return null;
        }
    }
}