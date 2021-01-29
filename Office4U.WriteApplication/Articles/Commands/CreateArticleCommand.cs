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

        public CreateArticleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(ArticleForCreationDto articleForCreation)
        {
            // TODO: inject correct project AutoMapper
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            var newArticle = mapper.Map<Domain.Model.Articles.Entities.Article>(articleForCreation);

            _unitOfWork.ArticleRepository.Add(newArticle);

            await _unitOfWork.Commit();

            // TODO: handle errors

            //if (await _unitOfWork.Commit())
            //{
            //    var articleToReturn = _mapper.Map<ArticleForReturnDto>(newArticle);
            //    return CreatedAtRoute("GetArticle", new { id = newArticle.Id }, articleToReturn);
            //}

            //return BadRequest("Failed to create article");


            // evt. notifications

        }
    }
}