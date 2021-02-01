using AutoMapper;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Helpers;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Commands
{
    public class CreateArticleCommand : ICreateArticleCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateArticleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ArticleForReturnDto> Execute(ArticleForCreationDto articleForCreation)
        {
            // TODO: inject correct project AutoMapper
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            var newArticle = mapper.Map<Article>(articleForCreation);

            _unitOfWork.ArticleRepository.Add(newArticle);

            if (await _unitOfWork.Commit())
            {
                var articleForReturnDto = mapper.Map<ArticleForReturnDto>(newArticle);
                return articleForReturnDto;
            }

            return null;
        }
    }
}