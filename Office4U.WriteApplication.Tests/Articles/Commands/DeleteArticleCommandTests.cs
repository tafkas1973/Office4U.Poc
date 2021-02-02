using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.UnitOfWork;
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
    public class DeleteArticleCommandTests : DatabaseFixture
    {
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<DataContext> _dataContextMock;
        private Mock<IArticleRepository> _articleRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void SetUp()
        {
            _testArticles = ArticleList.GetDefaultList();
            _articleDbSetMock = _testArticles.AsQueryable().BuildMockDbSet();
            _dataContextMock = new Mock<DataContext>();
            _dataContextMock.Setup(m => m.Articles).Returns(_articleDbSetMock.Object);
            _articleRepositoryMock = new Mock<IArticleRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.ArticleRepository).Returns(_articleRepositoryMock.Object);
            _unitOfWork = new UnitOfWork(TestContext, new ArticleRepository(TestContext), null);
        }

        [Test]
        public async Task Delete_ExistingObject_CorrectCallsPerformed()
        {
            //Arrange
            var id = 1;
            var deleteArticleCommand = new DeleteArticleCommand(_unitOfWorkMock.Object);
            var countBefore = TestContext.Articles.Count();

            //Act
            var result = await deleteArticleCommand.Execute(id);

            //Assert
            _articleRepositoryMock.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepositoryMock.Verify(r => r.Delete(It.IsAny<Article>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Test]
        public async Task Delete_ExistingObject_ObjectIsDeleted()
        {
            //Arrange
            var id = 1;
            var deleteArticleCommand = new DeleteArticleCommand(_unitOfWork);
            var countBefore = TestContext.Articles.Count();

            //Act
            var result = await deleteArticleCommand.Execute(id);

            //Assert
            Assert.That(TestContext.Articles.Count(), Is.EqualTo(countBefore - 1));
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Delete_NonExistingObject_ObjectIsNotDeleted()
        {
            //Arrange
            var id = 1;
            var deleteArticleCommand = new DeleteArticleCommand(_unitOfWorkMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await deleteArticleCommand.Execute(id);

            //Assert
            _articleRepositoryMock.Verify(r => r.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            _articleRepositoryMock.Verify(r => r.Delete(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
