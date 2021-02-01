using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Common;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.UnitOfWork;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using Office4U.Tests.Builders;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Articles.ImportExport.Api.Data.Repositories
{
    public class ArticleRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private IArticleRepository _articleRepository;
        private IReadOnlyArticleRepository _readOnlyArticleRepository;
        private List<Article> _testArticles;
        private Mock<DbSet<Article>> _articleDbSetMock;
        private Mock<DataContext> _dataContextMock;
        private Mock<ReadOnlyDataContext> _readOnlyDataContextMock;
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
            _readOnlyDataContextMock = new Mock<ReadOnlyDataContext>();
            _readOnlyDataContextMock.Setup(m => m.Articles).Returns(_articleDbSetMock.Object);

            _unitOfWork = new UnitOfWork(_dataContextMock.Object);
            _articleRepository = _unitOfWork.ArticleRepository;
            _readOnlyArticleRepository = new ReadOnlyArticleRepository(_readOnlyDataContextMock.Object);
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
