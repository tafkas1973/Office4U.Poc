using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Office4U.Common;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Presentation.Controller.Articles;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.ReadApplication.Helpers;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using Retail4U.Office4U.WebApi.Tools.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Articles.ImportExport.Api.Articles.Controllers
{
    public class ArticlesControllerTests : ControllerTestsBase
    {
        private ArticleParams _articleParams;
        private IEnumerable<Article> _testArticles;
        private ArticlesController _articlesController;

        private Mock<IGetArticlesQuery> _listQueryMock = new Mock<IGetArticlesQuery>();
        private Mock<IGetArticleQuery> _singleQueryMock = new Mock<IGetArticleQuery>();
        private Mock<ICreateArticleCommand> _createCommandMock = new Mock<ICreateArticleCommand>();
        private Mock<IUpdateArticleCommand> _updateCommandMock = new Mock<IUpdateArticleCommand>();
        private Mock<IDeleteArticleCommand> _deleteCommandMock = new Mock<IDeleteArticleCommand>();
        private PagedList<ArticleDto> _articlesDtoPagedList;



        // =======================================
        // TODO: review tests after DDD/CQRS setup
        // =======================================


        [SetUp]
        public void Setup()
        {
            _articleParams = new ArticleParams();
            _testArticles = new List<Article>() {
                new ArticleBuilder().WithId(1).WithCode("Article1").WithName1("1st article").WithSupplierId("sup1").WithSupplierReference("sup1 ref 1").WithUnit("ST").WithPurchasePrice(10.00M).Build(),
                new ArticleBuilder().WithId(2).WithCode("Article2").WithName1("2nd article").WithSupplierId("sup2").WithSupplierReference("sup1 ref 2").WithUnit("ST").WithPurchasePrice(20.00M).Build(),
                new ArticleBuilder().WithId(3).WithCode("Article3").WithName1("3rd article").WithSupplierId("sup3").WithSupplierReference("sup2 ref 1").WithUnit("ST").WithPurchasePrice(30.00M).Build()
            }.AsEnumerable();
            var articlesPagedList = new PagedList<Article>(items: _testArticles, count: 3, pageNumber: 1, pageSize: 10);

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>()));
            var articlesDtoList = mapper.Map<IEnumerable<ArticleDto>>(articlesPagedList);

            _articlesDtoPagedList = new PagedList<ArticleDto>(articlesDtoList, articlesPagedList.TotalCount, articlesPagedList.CurrentPage, articlesPagedList.PageSize);
            _listQueryMock
                .Setup(m => m.Execute(It.IsAny<ArticleParams>()))
                .Returns(Task.FromResult(_articlesDtoPagedList));

            _articlesController = new ArticlesController(
                _listQueryMock.Object,
                _singleQueryMock.Object,
                _createCommandMock.Object,
                _updateCommandMock.Object,
                _deleteCommandMock.Object)
            {
                ControllerContext = TestControllerContext
            };
        }

        [Test]
        public async Task GetArticles_WithDefaultPagingValues_ReturnsArticleDtoListWith3Items()
        {
            // arrange

            // act
            var result = await _articlesController.GetArticles(_articleParams);

            // assert
            _listQueryMock.Verify(m => m.Execute(It.IsAny<ArticleParams>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ActionResult<IEnumerable<ArticleDto>>)));
            Assert.That(result.Result.GetType(), Is.EqualTo(typeof(OkObjectResult)));
            Assert.That(((ObjectResult)result.Result).Value.GetType(), Is.EqualTo(typeof(PagedList<ArticleDto>)));
            Assert.That(((List<ArticleDto>)((ObjectResult)result.Result).Value).Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetArticle_WithIdEqualsTo2_ReturnsTheCorrectArticleDto()
        {
            // arrange
            _singleQueryMock
                .Setup(m => m.Execute(It.IsAny<int>()))
                .Returns(Task.FromResult(_articlesDtoPagedList[1]));

            // act
            var result = await _articlesController.GetArticle(2);

            // assert
            _singleQueryMock.Verify(m => m.Execute(It.IsAny<int>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ActionResult<ArticleDto>)));
            Assert.That(((ObjectResult)result.Result).Value.GetType(), Is.EqualTo(typeof(ArticleDto)));
            Assert.That(((ArticleDto)((ObjectResult)result.Result).Value).Id, Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateArticle_WithNonExistingId_ReturnsAnException()
        {
            // arrange
            var articleForUpdateDto = new ArticleForUpdateDto() { Id = 5, Name1 = "Article01 Updated" };

            // act
            var result = await _articlesController.UpdateArticle(articleForUpdateDto);

            // assert
            _updateCommandMock.Verify(m => m.Execute(It.IsAny<ArticleForUpdateDto>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(BadRequestObjectResult)));
            Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(400));
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Failed to update article"));
        }
    }
}
