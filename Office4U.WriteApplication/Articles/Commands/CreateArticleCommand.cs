using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
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

        public async Task Execute(ArticleForCreationDto articleForCreation)
        {
            // TODO mapping via DI !!           
            // var newArticle = _mapper.Map<Article>(newArticleDto);
            var newArticle = Domain.Model.Entities.Articles.Article.Create(
                articleForCreation.Code,
                articleForCreation.SupplierId,
                articleForCreation.SupplierReference,
                articleForCreation.Name1,
                articleForCreation.PurchasePrice,
                articleForCreation.Unit
                );

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
