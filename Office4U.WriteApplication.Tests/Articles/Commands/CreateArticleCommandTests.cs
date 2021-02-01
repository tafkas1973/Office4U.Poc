using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.UnitOfWork;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.ReadApplication.Helpers;
using Office4U.Tests.TestData;
using Office4U.WriteApplication.Articles.Commands;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Tests.Articles.Commands
{
    public class CreateArticleCommandTests : DatabaseFixture
    {
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<DataContext> _dataContextMock;
        private Mock<IArticleRepository> _articleRepository;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mapper _writeMapper;

        [SetUp]
        public void SetUp()
        {
            // TODO: find solution with in memory db, without need to mock context
            _testArticles = ArticleList.GetDefaultList();
            _articleDbSetMock = _testArticles.AsQueryable().BuildMockDbSet();
            _dataContextMock = new Mock<DataContext>();
            _dataContextMock.Setup(m => m.Articles).Returns(_articleDbSetMock.Object);
            _articleRepository = new Mock<IArticleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.ArticleRepository).Returns(_articleRepository.Object);
        }

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
            var articleForCreation = new ArticleForCreationDto();
            var createArticleCommand = new CreateArticleCommand(_unitOfWorkMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await createArticleCommand.Execute(articleForCreation);

            //Assert
            _articleRepository.Verify(r => r.Add(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.Null);
        }
    }
}
