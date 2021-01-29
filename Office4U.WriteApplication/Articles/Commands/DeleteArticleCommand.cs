using Office4U.WriteApplication.Article.Interfaces;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Article.Commands
{
    public class DeleteArticleCommand : IDeleteArticleCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteArticleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(int id)
        {
            var articleToDelete = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(id);

            _unitOfWork.ArticleRepository.Delete(articleToDelete);

            await _unitOfWork.Commit();

            // TODO: handle errors

            //if (await _unitOfWork.Commit())
            //    return Ok();

            //return BadRequest("Failed to delete article");
        }
    }
}
