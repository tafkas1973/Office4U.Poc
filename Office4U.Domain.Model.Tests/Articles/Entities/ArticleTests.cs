using NUnit.Framework;

namespace Office4U.Domain.Model.Articles.Entities
{
    public class ArticleTests
    {
        [Test]
        public void Create_CallConstructor_ReturnsNewArticleWithPropertiesSet()
        {
            //Arrange

            //Act
            var result = Article.Create("new code", "new name", "sup id", "sup ref", 123.45M, "ST");

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(Article)));
            Assert.That(result.Code, Is.EqualTo("new code"));
            Assert.That(result.Name1, Is.EqualTo("new name"));
            Assert.That(result.SupplierId, Is.EqualTo("sup id"));
            Assert.That(result.SupplierReference, Is.EqualTo("sup ref"));
            Assert.That(result.PurchasePrice, Is.EqualTo(123.45M));
            Assert.That(result.Unit, Is.EqualTo("ST"));
        }

        public void Create_SetPhoto_ThrowsNoExceptions()
        {
            //Arrange
            var article = Article.Create("new code", "new name", "sup id", "sup ref", 123.45M, "ST");

            //Act
            article.AddPhoto(ArticlePhoto.Create("www.google.be", true));

            //Assert
        }
    }
}
