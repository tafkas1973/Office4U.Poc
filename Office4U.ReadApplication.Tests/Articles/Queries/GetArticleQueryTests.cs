using AutoMapper;
using Moq;
using NUnit.Framework;
using Office4U.Common;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using Office4U.Tests.TestData;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.ReadApplication.Articles.Queries
{
    public class GetArticleQueryTests
    {
        [Test]
        public async Task GetArticle_ExecuteWithExistingId_ReturnsCorrectArticleDto()
        {
            //Arrange
            var articlesRepositoryMock = new Mock<IReadOnlyArticleRepository>();
            var articleParams = new ArticleParams();

            var testArticles = ArticleList.GetShortList().AsEnumerable();
            var id = 1;
            articlesRepositoryMock.Setup(r => r.GetArticleByIdAsync(id)).Returns(Task.FromResult(testArticles.First()));
            var readMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
            var getArticleQuery = new GetArticleQuery(articlesRepositoryMock.Object, readMapper);

            //Act
            var result = await getArticleQuery.Execute(id);

            //Assert
            articlesRepositoryMock.Verify(m => m.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ArticleDto)));
            Assert.That(result.Name1, Is.EqualTo(testArticles.First().Name1));
        }

        [Test]
        public async Task GetArticle_ExecuteWithNonExistingId_ReturnsNull()
        {
            //Arrange
            var articlesRepositoryMock = new Mock<IReadOnlyArticleRepository>();
            var articleParams = new ArticleParams();

            var testArticles = ArticleList.GetShortList().AsEnumerable();
            var id = 99;
            articlesRepositoryMock.Setup(r => r.GetArticleByIdAsync(id)).Returns(Task.FromResult<Article>(null));
            var readMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
            var getArticleQuery = new GetArticleQuery(articlesRepositoryMock.Object, readMapper);
   
            //Act
            var result = await getArticleQuery.Execute(id);

            //Assert
            articlesRepositoryMock.Verify(m => m.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Null);
        }
    }
}
