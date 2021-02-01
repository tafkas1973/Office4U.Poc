using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.UnitOfWork
{
    public class UnitOfWorkTests
    {
        private ArticleRepository _articleRepository;
        private UnitOfWork _unitOfWork;
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<DataContext> _dataContextMock;

        [SetUp]
        public void Setup()
        {
            _testArticles = ArticleList.GetDefaultList();
            _articleDbSetMock = _testArticles.AsQueryable().BuildMockDbSet();
            _dataContextMock = new Mock<DataContext>();
            _dataContextMock.Setup(m => m.Articles).Returns(_articleDbSetMock.Object);
            _articleRepository = new ArticleRepository(_dataContextMock.Object);
            _unitOfWork = new UnitOfWork(_dataContextMock.Object, _articleRepository, null);
        }

        [Test]
        public async Task Commit_WithValidConditions_PerformsCommit()
        {
            //Arrange
            _testArticles.First().SetCode("a changed code");
            _articleRepository.Update(_testArticles.First());
            _dataContextMock.Setup(m => m.SaveChangesAsync(new CancellationToken())).Returns(Task.FromResult(1));

            //Act
            var result = await _unitOfWork.Commit();

            //Assert
            _dataContextMock.Verify(m => m.SaveChangesAsync(new CancellationToken()), Times.Once);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Commit_WithInvalidConditions_DoesNotPerformCommit()
        {
            //Arrange
            _testArticles.First().SetCode("a changed code");
            _articleRepository.Update(_testArticles.First());
            _dataContextMock.Setup(m => m.SaveChangesAsync(new CancellationToken())).Returns(Task.FromResult(0));

            //Act
            var result = await _unitOfWork.Commit();

            //Assert
            _dataContextMock.Verify(m => m.SaveChangesAsync(new CancellationToken()), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}
