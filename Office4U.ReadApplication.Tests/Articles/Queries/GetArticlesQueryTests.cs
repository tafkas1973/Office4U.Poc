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
    public class GetArticlesQueryTests
    {
        private Mock<IReadOnlyArticleRepository> _articlesRepositoryMock;
        private ArticleParams _articleParams;
        private IEnumerable<Article> _testArticles;
        private PagedList<Article> _pagedListOfArticles;

        [SetUp]
        public void SetUp ()
        {
            _articlesRepositoryMock = new Mock<IReadOnlyArticleRepository>();
            _articleParams = new ArticleParams();
            _testArticles = ArticleList.GetDefaultList().AsEnumerable();
            _pagedListOfArticles = new PagedList<Article>(_testArticles, _testArticles.Count(), 1, 10);
        }

        [Test]
        public async Task Execute_WithDefaultParams_ReturnsPagedListOfArticleDtos()
        {
            //Arrange
            _articlesRepositoryMock.Setup(r => r.GetArticlesAsync(_articleParams)).Returns(Task.FromResult(_pagedListOfArticles));
            var getArticlesQuery = new GetArticlesQuery(_articlesRepositoryMock.Object);

            //Act
            var result = await getArticlesQuery.Execute(_articleParams);

            //Assert
            _articlesRepositoryMock.Verify(m => m.GetArticlesAsync(It.IsAny<ArticleParams>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<ArticleDto>)));
            Assert.That(result.Count, Is.EqualTo(_testArticles.Count()));
        }
    }
}
