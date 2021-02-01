using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.UnitOfWork;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.TestData;
using Office4U.WriteApplication.Articles.Commands;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Tests.Articles.Commands
{
    public class CreateArticleCommandTests : DatabaseFixture
    {
        [Test]
        public async Task Create_ValidObject_ReturnsNewArticle()
        {
            //Arrange
            var unitOfWork = new UnitOfWork(TestContext, new ArticleRepository(TestContext), null);

            var articleForCreation = new ArticleForCreationDto()
            {
                Code = "new code",
                Name1 = "new name",
                SupplierId = "sup id",
                SupplierReference = "sup ref",
                PurchasePrice = 99.99M,
                Unit = "ST"
            };            
            var createArticleCommand = new CreateArticleCommand(unitOfWork);
            var articleCountBefore = TestContext.Articles.Count();

            //Act
            var result = await createArticleCommand.Execute(articleForCreation);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ArticleForReturnDto)));
            Assert.That(result.Id, Is.Not.Null);
            Assert.That(result.Name1, Is.EqualTo(articleForCreation.Name1));
            Assert.That(TestContext.Articles.Count, Is.EqualTo(articleCountBefore + 1));
        }

        [Test]
        public async Task Create_InvalidObject_ReturnsNull()
        {
            //Arrange
            var testArticles = ArticleList.GetDefaultList();
            var articleDbSetMock = testArticles.AsQueryable().BuildMockDbSet();
            var dataContextMock = new Mock<DataContext>();
            dataContextMock.Setup(m => m.Articles).Returns(articleDbSetMock.Object);
            var articleRepository = new Mock<IArticleRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.ArticleRepository).Returns(articleRepository.Object);

            var articleForCreation = new ArticleForCreationDto();
            var createArticleCommand = new CreateArticleCommand(unitOfWorkMock.Object);
            unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await createArticleCommand.Execute(articleForCreation);

            //Assert
            articleRepository.Verify(r => r.Add(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.Null);
        }
    }
}
