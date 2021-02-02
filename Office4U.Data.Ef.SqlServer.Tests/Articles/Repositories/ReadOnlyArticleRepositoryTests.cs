using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Common;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.TestData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Articles.Repositories
{
    public class ReadOnlyArticleRepositoryTests : DatabaseFixture
    {
        private ReadOnlyArticleRepository _readOnlyArticleRepository;
        private readonly int _defaultPageSize = 10;

        [SetUp]
        public void Setup()
        {
            // ArticleList.GetDefaultList();
            // sort by name1 : 10th article/11th article/12th article/1st article/2nd article/3rd article/4th article/5th article/6th article/7th article/8th article/9th article
            // sort by supref : sup1 ref 1/sup1 ref 2/sup2 ref 1/sup2 ref 2/sup3 ref 1/sup3 ref 2/sup4 ref 1/sup4 ref 2/sup5 ref 1/sup5 ref 2/sup6 ref 1/sup6 ref 2
            _readOnlyArticleRepository = new ReadOnlyArticleRepository(TestReadOnlyContext);
        }

        [Test]
        public async Task GetArticlesAsync_WithDefaultParams_ReturnsArticlesThatMatchCriteria()
        {
            //Arrange
            var articleParams = new ArticleParams();

            //Act
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PagedList<Article>)));
            Assert.That(result.First().Photos.GetType(), Is.EqualTo(typeof(HashSet<ArticlePhoto>)));
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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticlesAsync(articleParams);

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
            var result = await _readOnlyArticleRepository.GetArticleByIdAsync(3);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(Article)));
            Assert.That(result.Photos.GetType(), Is.EqualTo(typeof(HashSet<ArticlePhoto>)));
            Assert.That(result.Code, Is.EqualTo("Article03"));
        }

        [Test]
        public async Task GetArticleByIdAsync_WithNonExistingId_ReturnsNull()
        {
            //Arrange

            //Act
            var result = await _readOnlyArticleRepository.GetArticleByIdAsync(99);

            //Assert
            Assert.That(result, Is.Null);
        }
    }
}
