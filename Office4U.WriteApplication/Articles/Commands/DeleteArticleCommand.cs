using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Articles.Commands
{
    public class DeleteArticleCommand : IDeleteArticleCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteArticleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Execute(int id)
        {
            var articleToDelete = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(id);

            _unitOfWork.ArticleRepository.Delete(articleToDelete);

            if (await _unitOfWork.Commit()) return true;

            return false;
        }
    }
}
