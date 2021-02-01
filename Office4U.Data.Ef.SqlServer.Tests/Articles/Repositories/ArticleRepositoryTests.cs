using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Articles.Repositories
{
    public class ArticleRepositoryTests
    {
        private ArticleRepository _articleRepository;
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
        }

        [Test]
        public async Task GetArticleByIdAsync_WithExistingId_ReturnsArticle()
        {
            //Arrange

            //Act
            var result = await _articleRepository.GetArticleByIdAsync(3);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(Article)));
            Assert.That(result.Photos.GetType(), Is.EqualTo(typeof(List<ArticlePhoto>)));
            Assert.That(result.Code, Is.EqualTo("Article03"));
        }

        [Test]
        public async Task GetArticleByIdAsync_WithNonExistingId_ReturnsNull()
        {
            //Arrange

            //Act
            var result = await _articleRepository.GetArticleByIdAsync(99);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        //[Ignore("Problem with generic in setup")]
        public void Update_WithChangedEntity_PerformsAContextUpdate()
        {
            //Arrange
            var updatedArticle = _testArticles.First();
            updatedArticle.Code = "1st article updated";
            var isContextUpdateCalled = false;
            //_dataContextMock.Setup(m => m.Set<Article>()).Returns(_articleDbSetMock.Object).Verifiable();
            _dataContextMock.Setup(m => m.Update(It.IsAny<object>())).Callback(() => isContextUpdateCalled = true);

            //Act
            _articleRepository.Update(updatedArticle);

            //Assert            
            //_dataContextMock.Verify(m => m.Update(updatedArticle), Times.Once); //doesn't work with generic type
            Assert.That(isContextUpdateCalled, Is.True);
        }
    }
}
