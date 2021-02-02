using NUnit.Framework;

namespace Office4U.Domain.Model.Articles.Entities
{
    public class ArticlePhotoTests
    {
        [Test]
        public void ArticlePhoto_Create_ReturnsArticlePhoto()
        {
            //Arrange

            //Act
            var result = ArticlePhoto.Create("www.google.be", true);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ArticlePhoto)));
        }
    }
}
