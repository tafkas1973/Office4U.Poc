using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
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
    public class CreateArticleCommandTests
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
            _writeMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
        }

        [Test]
        public void Create_ValidObject_ReturnsNewArticle()
        {
            //Arrange
            var articleForCreation = new ArticleForCreationDto()
            {
                Code = "new code",
                Name1 = "new name",
                SupplierId = "sup id",
                SupplierReference = "sup ref",
                PurchasePrice = 99.99M,
                Unit = "ST"
            };
            var createArticleCommand = new CreateArticleCommand(_unitOfWorkMock.Object, _writeMapper);

            //Act
            var result = createArticleCommand.Execute(articleForCreation);

            //Assert
            _articleRepository.Verify(r => r.Add(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result.GetType(), Is.EqualTo(Task.FromResult(typeof(ArticleForReturnDto))));
            Assert.That(result.Id, Is.EqualTo(0));
        }
    }
}
