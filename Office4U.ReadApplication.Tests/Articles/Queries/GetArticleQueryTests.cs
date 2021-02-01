using AutoMapper;
using Moq;
using NUnit.Framework;
using Office4U.Common;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using Office4U.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Queries
{
    public class GetArticleQueryTests
    {
        private Mock<IReadOnlyArticleRepository> _articlesRepositoryMock;
        private IEnumerable<Article> _testArticles;

        [SetUp]
        public void SetUp()
        {
            _articlesRepositoryMock = new Mock<IReadOnlyArticleRepository>();
            var articleParams = new ArticleParams();
            _testArticles = ArticleList.GetShortList().AsEnumerable();
        }

        [Test]
        public async Task Execute_WithExistingId_ReturnsCorrectArticleDto()
        {
            //Arrange
            var id = 1;
            _articlesRepositoryMock.Setup(r => r.GetArticleByIdAsync(id)).Returns(Task.FromResult(_testArticles.First()));
            var readMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
            var getArticleQuery = new GetArticleQuery(_articlesRepositoryMock.Object, readMapper);

            //Act
            var result = await getArticleQuery.Execute(id);

            //Assert
            _articlesRepositoryMock.Verify(m => m.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ArticleDto)));
            Assert.That(result.Name1, Is.EqualTo(_testArticles.First().Name1));
        }

        [Test]
        public async Task Execute_WithNonExistingId_ReturnsNull()
        {
            //Arrange
            var id = 99;
            _articlesRepositoryMock.Setup(r => r.GetArticleByIdAsync(id)).Returns(Task.FromResult<Article>(null));
            var readMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
            var getArticleQuery = new GetArticleQuery(_articlesRepositoryMock.Object, readMapper);
   
            //Act
            var result = await getArticleQuery.Execute(id);

            //Assert
            _articlesRepositoryMock.Verify(m => m.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Null);
        }
    }
}
