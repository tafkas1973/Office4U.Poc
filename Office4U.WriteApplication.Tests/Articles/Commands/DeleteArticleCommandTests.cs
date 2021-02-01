using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.TestData;
using Office4U.WriteApplication.Articles.Commands;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Tests.Articles.Commands
{
    public class DeleteArticleCommandTests
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
        public async Task Delete_ExistingObject_ReturnsTrue()
        {
            //Arrange
            var id = 1;
            var deleteArticleCommand = new DeleteArticleCommand(_unitOfWorkMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await deleteArticleCommand.Execute(id);

            //Assert
            _articleRepository.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepository.Verify(r => r.Delete(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Delete_NonExistingObject_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var deleteArticleCommand = new DeleteArticleCommand(_unitOfWorkMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await deleteArticleCommand.Execute(id);

            //Assert
            _articleRepository.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepository.Verify(r => r.Delete(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
