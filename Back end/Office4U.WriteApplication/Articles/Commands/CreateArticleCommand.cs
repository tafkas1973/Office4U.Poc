using AutoMapper;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Commands
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
            var newArticle = _mapper.Map<Article>(articleForCreation);

            if (!string.IsNullOrEmpty(articleForCreation.PhotoUrl))
                newArticle.Photos.Add(ArticlePhoto.Create(articleForCreation.PhotoUrl, true));

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