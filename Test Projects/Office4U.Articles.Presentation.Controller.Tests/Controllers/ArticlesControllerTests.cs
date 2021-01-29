using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Office4U.Articles.Common;
using Office4U.Articles.Domain.Model.Entities.Articles;
using Office4U.Articles.ImportExport.Api.Controllers;
using Office4U.Articles.Presentation.Controller.Controllers;
using Office4U.Articles.ReadApplication.Article.DTOs;
using Office4U.Articles.ReadApplication.Article.Interfaces;
using Office4U.Articles.ReadApplication.Helpers;
using Office4U.Articles.WriteApplication.Article.DTOs;
using Office4U.Articles.WriteApplication.Article.Interfaces;
using Office4U.Articles.WriteApplication.Article.Interfaces.IOC;
using Office4U.Articles.WriteApplication.Interfaces.IOC;
using Retail4U.Office4U.WebApi.Tools.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Articles.ImportExport.Api.Tests.Controllers
{
    public class ArticlesControllerTests : ControllerTestsBase
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IArticleRepository> _articleRepositoryMock;
        private ArticleParams _articleParams;
        private IEnumerable<Article> _testArticles;
        private ArticlesController _articlesController;

        private Mock<IGetArticlesListQuery> _listQueryMock = new Mock<IGetArticlesListQuery>();
        private Mock<IGetArticleQuery> _singleQueryMock = new Mock<IGetArticleQuery>();
        private Mock<ICreateArticleCommand> _createCommandMock = new Mock<ICreateArticleCommand>();
        private Mock<IUpdateArticleCommand> _updateCommandMock = new Mock<IUpdateArticleCommand>();
        private Mock<IDeleteArticleCommand> _deleteCommandMock = new Mock<IDeleteArticleCommand>();


        [SetUp]
        public void Setup()
        {
            _articleRepositoryMock = new Mock<IArticleRepository>() { DefaultValue = DefaultValue.Mock };
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(m => m.ArticleRepository).Returns(_articleRepositoryMock.Object);
            _articleParams = new ArticleParams();

            _testArticles = new List<Article>() {
                new ArticleBuilder().WithId(1).WithCode("Article1").WithName1("1st article").WithSupplierId("sup1").WithSupplierReference("sup1 ref 1").WithUnit("ST").WithPurchasePrice(10.00M).Build(),
                new ArticleBuilder().WithId(2).WithCode("Article2").WithName1("2nd article").WithSupplierId("sup2").WithSupplierReference("sup1 ref 2").WithUnit("ST").WithPurchasePrice(20.00M).Build(),
                new ArticleBuilder().WithId(3).WithCode("Article3").WithName1("3rd article").WithSupplierId("sup3").WithSupplierReference("sup2 ref 1").WithUnit("ST").WithPurchasePrice(30.00M).Build()
            }.AsEnumerable();
            var articlesPagedList = new PagedList<Article>(items: _testArticles, count: 3, pageNumber: 1, pageSize: 10);

            _articleRepositoryMock
                .Setup(m => m.GetArticlesAsync(_articleParams))
                .ReturnsAsync(articlesPagedList);
            _articleRepositoryMock
                .Setup(m => m.GetArticleByIdAsync(2))
                .ReturnsAsync(articlesPagedList[1]);

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            _articlesController = new ArticlesController(_listQueryMock.Object, _singleQueryMock.Object, _createCommandMock.Object, _updateCommandMock.Object, _deleteCommandMock.Object);
        }

        [Test]
        public async Task GetArticles_WithDefaultPagingValues_ReturnsArticleDtoListWith3Items()
        {
            // arrange

            // act
            var result = await _articlesController.GetArticles(_articleParams);

            // assert
            _articleRepositoryMock.Verify(m => m.GetArticlesAsync(It.IsAny<ArticleParams>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ActionResult<IEnumerable<ArticleDto>>)));
            Assert.That(result.Result.GetType(), Is.EqualTo(typeof(OkObjectResult)));
            Assert.That(((ObjectResult)result.Result).Value.GetType(), Is.EqualTo(typeof(List<ArticleDto>)));
            Assert.That(((List<ArticleDto>)((ObjectResult)result.Result).Value).Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetArticle_WithIdEqualsTo2_ReturnsTheCorrectArticleDto()
        {
            // arrange

            // act
            var result = await _articlesController.GetArticle(2);

            // assert
            _articleRepositoryMock.Verify(m => m.GetArticleByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ActionResult<ArticleDto>)));
            Assert.That(result.Value.GetType(), Is.EqualTo(typeof(ArticleDto)));
            Assert.That(result.Value.Id, Is.EqualTo(2));
        }

        [Test]
        public async Task GetArticle_WithNonExistingId_ReturnsTheCorrectArticleDto()
        {
            // arrange
            var articleForUpdateDto = new ArticleForUpdateDto() { Id = 5, Name1 = "Article01 Updated" };

            // act
            var result = await _articlesController.UpdateArticle(articleForUpdateDto);

            // assert
            _articleRepositoryMock.Verify(m => m.Update(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(BadRequestObjectResult)));
            Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(400));
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Failed to update article"));
        }
    }
}
