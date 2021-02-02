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
    public class UpdateArticleCommandTests : DatabaseFixture
    {
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<CommandDbContext> _dataContextMock;
        private Mock<IArticleRepository> _articleRepositoryMock;
        private IArticleRepository _articleRepository;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mapper _writeMapper;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _testArticles = ArticleList.GetDefaultList();
            _articleDbSetMock = _testArticles.AsQueryable().BuildMockDbSet();
            _dataContextMock = new Mock<CommandDbContext>();
            _dataContextMock.Setup(m => m.Articles).Returns(_articleDbSetMock.Object);
            _articleRepositoryMock = new Mock<IArticleRepository>();
            _articleRepository = new ArticleRepository(TestContext);
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.ArticleRepository).Returns(_articleRepositoryMock.Object);
            _unitOfWork = new UnitOfWork(TestContext, _articleRepository, null);
            _writeMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
        }

        [Test]
        public async Task Update_ValidObject_CorrectCallsPerformed()
        {
            //Arrange
            var articleForUpdate = new ArticleForUpdateDto()
            {
                Id = 1,
                Name1 = "1st article Updated",
                SupplierId = "sup id",
                SupplierReference = "sup ref",
                PurchasePrice = 99.99M
            };
            var updateArticleCommand = new UpdateArticleCommand(_unitOfWorkMock.Object, _writeMapper);
            _articleRepositoryMock.Setup(r => r.GetArticleByIdAsync(articleForUpdate.Id)).Returns(Task.FromResult(_testArticles.First()));
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await updateArticleCommand.Execute(articleForUpdate);

            //Assert
            _articleRepositoryMock.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepositoryMock.Verify(r => r.Update(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(_testArticles.First().Name1, Is.EqualTo(articleForUpdate.Name1));
        }

        [Test]
        public async Task Update_ValidObject_ObjectIsUpdated()
        {
            //Arrange
            var articleForUpdate = new ArticleForUpdateDto()
            {
                Id = 1,
                Name1 = "1st article Updated",
                SupplierId = "sup id",
                SupplierReference = "sup ref",
                PurchasePrice = 99.99M
            };
            var updateArticleCommand = new UpdateArticleCommand(_unitOfWork, _writeMapper);

            //Act
            var result = await updateArticleCommand.Execute(articleForUpdate);

            //Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(TestContext.Articles.First().Name1, Is.EqualTo(articleForUpdate.Name1));
        }

        [Test]
        public async Task Update_InvalidObject_ReturnsFalse()
        {
            //Arrange
            var articleForUpdate = new ArticleForUpdateDto()
            {
                Id = 999,
                Name1 = "1st article Updated",
                SupplierId = "sup id",
                SupplierReference = "sup ref",
                PurchasePrice = 99.99M
            };
            var updateArticleCommand = new UpdateArticleCommand(_unitOfWorkMock.Object, _writeMapper);
            _articleRepositoryMock.Setup(r => r.GetArticleByIdAsync(articleForUpdate.Id)).Returns(Task.FromResult(_testArticles.First()));
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await updateArticleCommand.Execute(articleForUpdate);

            //Assert
            _articleRepositoryMock.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepositoryMock.Verify(r => r.Update(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
