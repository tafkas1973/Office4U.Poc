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
    public class ArticleRepositoryTests : DatabaseFixture
    {
        [Test]
        public async Task GetArticleByIdAsync_WithExistingId_ReturnsArticle()
        {
            //Arrange
            var readOnlyArticleRepository = new ReadOnlyArticleRepository(TestReadOnlyContext);

            //Act
            var result = await readOnlyArticleRepository.GetArticleByIdAsync(3);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(Article)));
            Assert.That(result.Photos.GetType(), Is.EqualTo(typeof(List<ArticlePhoto>)));
            Assert.That(result.Code, Is.EqualTo("Article03"));
        }

        [Test]
        public async Task GetArticleByIdAsync_WithNonExistingId_ReturnsNull()
        {
            //Arrange
            var readOnlyArticleRepository = new ReadOnlyArticleRepository(TestReadOnlyContext);

            //Act
            var result = await readOnlyArticleRepository.GetArticleByIdAsync(99);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        //[Ignore("Problem with generic in setup")]
        public void Update_WithChangedEntity_PerformsAContextUpdate()
        {
            //Arrange
            var testArticles = ArticleList.GetDefaultList();
            var articleDbSetMock = testArticles.AsQueryable().BuildMockDbSet();
            var dataContextMock = new Mock<CommandDbContext>();
            dataContextMock.Setup(m => m.Articles).Returns(articleDbSetMock.Object);
            var articleRepository = new ArticleRepository(dataContextMock.Object);

            var updatedArticle = testArticles.First();
            updatedArticle.SetCode("1st article updated");
            var isContextUpdateCalled = false;
            //_dataContextMock.Setup(m => m.Set<Article>()).Returns(_articleDbSetMock.Object).Verifiable();
            dataContextMock.Setup(m => m.Update(It.IsAny<object>())).Callback(() => isContextUpdateCalled = true);

            //Act
            articleRepository.Update(updatedArticle);

            //Assert            
            //_dataContextMock.Verify(m => m.Update(updatedArticle), Times.Once); //doesn't work with generic type
            Assert.That(isContextUpdateCalled, Is.True);
        }
    }
}
