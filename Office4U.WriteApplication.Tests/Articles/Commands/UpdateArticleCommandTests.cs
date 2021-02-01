using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
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
    public class UpdateArticleCommandTests
    {
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<DataContext> _dataContextMock;
        private Mock<IArticleRepository> _articleRepository;
        private Mock<IUnitOfWork> _unitOfWorkMock;

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
        public async Task Update_ValidObject_ReturnsTrue()
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
            var updateArticleCommand = new UpdateArticleCommand(_unitOfWorkMock.Object);
            _articleRepository.Setup(r => r.GetArticleByIdAsync(articleForUpdate.Id)).Returns(Task.FromResult(_testArticles.First()));
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await updateArticleCommand.Execute(articleForUpdate);

            //Assert
            _articleRepository.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepository.Verify(r => r.Update(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(true));
            Assert.That(_testArticles.First().Name1, Is.EqualTo(articleForUpdate.Name1));
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
            var updateArticleCommand = new UpdateArticleCommand(_unitOfWorkMock.Object);
            _articleRepository.Setup(r => r.GetArticleByIdAsync(articleForUpdate.Id)).Returns(Task.FromResult(_testArticles.First()));
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await updateArticleCommand.Execute(articleForUpdate);

            //Assert
            _articleRepository.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepository.Verify(r => r.Update(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
