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
    public class GetArticlesQueryTests
    {
        [Test]
        public async Task GetArticles_ExecuteWithDefaultParams_ReturnsPagedListOfArticleDtos()
        {
            //Arrange
            var articlesRepositoryMock = new Mock<IReadOnlyArticleRepository>();
            var articleParams = new ArticleParams();

            var testArticles = ArticleList.GetDefaultList().AsEnumerable();
            var pagedListOfArticles = new PagedList<Article>(testArticles, testArticles.Count(), 1, 10);
            articlesRepositoryMock.Setup(r => r.GetArticlesAsync(articleParams)).Returns(Task.FromResult(pagedListOfArticles));
            var getArticlesQuery = new GetArticlesQuery(articlesRepositoryMock.Object);

            //Act
            var result = await getArticlesQuery.Execute(articleParams);

            //Assert
            articlesRepositoryMock.Verify(m => m.GetArticlesAsync(It.IsAny<ArticleParams>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<ArticleDto>)));
            Assert.That(result.Count, Is.EqualTo(testArticles.Count()));
        }
    }
}
