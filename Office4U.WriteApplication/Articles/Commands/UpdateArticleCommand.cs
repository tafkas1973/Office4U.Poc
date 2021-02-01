using AutoMapper;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Helpers;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Commands
{
    public class UpdateArticleCommand : IUpdateArticleCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateArticleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(ArticleForUpdateDto articleForUpdate)
        {
            Article article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(articleForUpdate.Id);

            // TODO: inject (correct) project AutoMapper
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            mapper.Map(articleForUpdate, article);

            _unitOfWork.ArticleRepository.Update(article);

            if (await _unitOfWork.Commit()) return true;

            return false;
        }
    }
}
