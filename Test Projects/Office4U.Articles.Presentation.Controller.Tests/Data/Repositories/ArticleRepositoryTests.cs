using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Articles.Common;
using Office4U.Articles.Data.Ef.SqlServer.Contexts;
using Office4U.Articles.Data.Ef.SqlServer.UnitOfWork;
using Office4U.Articles.Domain.Model.Entities.Articles;
using Office4U.Articles.WriteApplication.Article.Interfaces.IOC;
using Office4U.Articles.WriteApplication.Interfaces.IOC;
using Retail4U.Office4U.WebApi.Tools.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Articles.ImportExport.Api.Data.Repositories
{
    public class ArticleRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private IArticleRepository _articleRepository;
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<DataContext> _dataContextMock;
        private readonly int _defaultPageSize = 10;

        [SetUp]
        public void Setup()
        {
            _testArticles = new List<Article>() {
                new ArticleBuilder().WithId(1) .WithCode("Article01").WithName1("1st article") .WithSupplierId("sup1") .WithSupplierReference("sup2 ref 1").WithUnit("ST").WithPurchasePrice(10.00M).Build(),
                new ArticleBuilder().WithId(2) .WithCode("Article02").WithName1("2nd article") .WithSupplierId("sup2") .WithSupplierReference("sup2 ref 2").WithUnit("ST").WithPurchasePrice(20.00M).Build(),
                new ArticleBuilder().WithId(3) .WithCode("Article03").WithName1("3rd article") .WithSupplierId("sup3") .WithSupplierReference("sup4 ref 1").WithUnit("ST").WithPurchasePrice(30.00M).Build(),
                new ArticleBuilder().WithId(4) .WithCode("Article04").WithName1("4th article") .WithSupplierId("sup4") .WithSupplierReference("sup4 ref 2").WithUnit("ST").WithPurchasePrice(40.00M).Build(),
                new ArticleBuilder().WithId(5) .WithCode("Article05").WithName1("5th article") .WithSupplierId("sup5") .WithSupplierReference("sup6 ref 1").WithUnit("BS").WithPurchasePrice(50.00M).Build(),
                new ArticleBuilder().WithId(6) .WithCode("Article06").WithName1("6th article") .WithSupplierId("sup6") .WithSupplierReference("sup6 ref 2").WithUnit("ST").WithPurchasePrice(60.00M).Build(),
                new ArticleBuilder().WithId(7) .WithCode("Article07").WithName1("7th article") .WithSupplierId("sup7") .WithSupplierReference("sup1 ref 1").WithUnit("BS").WithPurchasePrice(70.00M).Build(),
                new ArticleBuilder().WithId(8) .WithCode("Article08").WithName1("8th article") .WithSupplierId("sup8") .WithSupplierReference("sup1 ref 2").WithUnit("ST").WithPurchasePrice(80.00M).Build(),
                new ArticleBuilder().WithId(9) .WithCode("Article09").WithName1("9th article") .WithSupplierId("sup9") .WithSupplierReference("sup3 ref 1").WithUnit("ST").WithPurchasePrice(90.00M).Build(),
                new ArticleBuilder().WithId(10).WithCode("Article10").WithName1("10th article").WithSupplierId("sup10").WithSupplierReference("sup3 ref 2").WithUnit("BX").WithPurchasePrice(100.00M).Build(),
                new ArticleBuilder().WithId(11).WithCode("Article11").WithName1("11th article").WithSupplierId("sup11").WithSupplierReference("sup5 ref 1").WithUnit("BM").WithPurchasePrice(110.00M).Build(),
                new ArticleBuilder().WithId(12).WithCode("Article12").WithName1("12th article").WithSupplierId("sup12").WithSupplierReference("sup5 ref 2").WithUnit("BX").WithPurchasePrice(120.00M).Build()
            };
            // sort by name1 : 10th article/11th article/12th article/1st article/2nd article/3rd article/4th article/5th article/6th article/7th article/8th article/9th article
            // sort by supref : sup1 ref 1/sup1 ref 2/sup2 ref 1/sup2 ref 2/sup3 ref 1/sup3 ref 2/sup4 ref 1/sup4 ref 2/sup5 ref 1/sup5 ref 2/sup6 ref 1/sup6 ref 2

            _articleDbSetMock = _testArticles.AsQueryable().BuildMockDbSet();

            _dataContextMock = new Mock<DataContext>();
            _dataContextMock.Setup(m => m.Articles).Returns(_articleDbSetMock.Object);

            _unitOfWork = new UnitOfWork(_dataContextMock.Object);
            _articleRepository = _unitOfWork.ArticleRepository;
        }

        [Test]
        public async Task GetArticlesAsync_WithDefaultParams_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams();

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.First().Photos.GetType(), Is.EqualTo(typeof(List<ArticlePhoto>)));
            Assert.That(result.Count, Is.EqualTo(_defaultPageSize));
            Assert.That(result.First().Code, Is.EqualTo("Article01"));
            Assert.That(result[9].Code, Is.EqualTo("Article10"));
        }


        // --- PAGING ---
        [Test]
        public async Task GetArticlesAsync_WithPageSize5TakePage2_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { PageSize = 5, PageNumber = 2 };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Code, Is.EqualTo("Article06"));
            Assert.That(result[4].Code, Is.EqualTo("Article10"));
        }

        [Test]
        public async Task GetArticlesAsync_WithPageSize5TakeLastPage_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { PageSize = 5, PageNumber = 3 };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Code, Is.EqualTo("Article11"));
            Assert.That(result[1].Code, Is.EqualTo("Article12"));
        }


        // --- SORTING ---
        [Test]
        public async Task GetArticlesAsync_WithOrderedByCode_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { OrderBy = "code" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(_defaultPageSize));
            Assert.That(result.First().Code, Is.EqualTo("Article01"));
            Assert.That(result[9].Code, Is.EqualTo("Article10"));
        }

        [Test]
        public async Task GetArticlesAsync_WithOrderBySupplierRef_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { OrderBy = "supplierReference" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(_defaultPageSize));
            Assert.That(result.First().SupplierReference, Is.EqualTo("sup1 ref 1"));
            Assert.That(result[9].SupplierReference, Is.EqualTo("sup5 ref 2"));
        }

        [Test]
        public async Task GetArticlesAsync_WithOrderByName1_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { OrderBy = "name" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(_defaultPageSize));
            Assert.That(result.First().Name1, Is.EqualTo("10th article"));
            Assert.That(result[9].Name1, Is.EqualTo("7th article"));
        }


        // --- FILTERING: Code, SupplierId, SupplierReference, Name1, Unit, PurchasePriceMin, PurchasePriceMax ---
        [Test]
        public async Task GetArticlesAsync_WithFilterCodeArt_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { Code = "aRt" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(_defaultPageSize));
            Assert.That(result.TotalCount, Is.EqualTo(12));
        }

        [Test]
        public async Task GetArticlesAsync_WithFilterCodeCle1_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { Code = "cLe1" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.First().Code, Is.EqualTo("Article10"));
            Assert.That(result[1].Code, Is.EqualTo("Article11"));
            Assert.That(result[2].Code, Is.EqualTo("Article12"));
        }


        [Test]
        public async Task GetArticlesAsync_WithFilterSupplierId_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { SupplierId = "SUP1" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.First().Code, Is.EqualTo("Article01"));
            Assert.That(result[3].Code, Is.EqualTo("Article12"));
        }


        [Test]
        public async Task GetArticlesAsync_WithFilterSupplierReference_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { SupplierReference = "SUP1" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Code, Is.EqualTo("Article07"));
            Assert.That(result[1].Code, Is.EqualTo("Article08"));
        }


        [Test]
        public async Task GetArticlesAsync_WithFilterName1_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { Name1 = "rd art" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Code, Is.EqualTo("Article03"));
        }


        [Test]
        public async Task GetArticlesAsync_WithFilterPurchasePriceMin_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { PurchasePriceMin = 50.00M };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(8));
            Assert.That(result.First().Code, Is.EqualTo("Article05"));
            Assert.That(result[7].Code, Is.EqualTo("Article12"));
        }

        [Test]
        public async Task GetArticlesAsync_WithFilterPurchasePriceMax_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { PurchasePriceMax = 80.00M };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(8));
            Assert.That(result.First().Code, Is.EqualTo("Article01"));
            Assert.That(result[7].Code, Is.EqualTo("Article08"));
        }


        [Test]
        public async Task GetArticlesAsync_WithFilterPurchasePriceMinAndMax_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { PurchasePriceMin = 50.00M, PurchasePriceMax = 80.00M };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.First().Code, Is.EqualTo("Article05"));
            Assert.That(result[3].Code, Is.EqualTo("Article08"));
        }


        [Test]
        public async Task GetArticlesAsync_WithFilterUnit_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams() { Unit = "B" };

            //Act
            var result = await _articleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Code, Is.EqualTo("Article05"));
            Assert.That(result[4].Code, Is.EqualTo("Article12"));
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
            updatedArticle.Code = "Article01 updated";
            var isContextUpdateCalled = false;
            //_dataContextMock.Setup(m => m.Set<Article>()).Returns(_articleDbSetMock.Object).Verifiable();
            _dataContextMock.Setup(m => m.Update(It.IsAny<object>())).Callback(() => isContextUpdateCalled = true);

            //Act
            _articleRepository.Update(updatedArticle);

            //Assert            
            //_dataContextMock.Verify(m => m.Update(updatedArticle), Times.Once); doesn't work with generic type
            Assert.That(isContextUpdateCalled, Is.True);
        }
    }
}
