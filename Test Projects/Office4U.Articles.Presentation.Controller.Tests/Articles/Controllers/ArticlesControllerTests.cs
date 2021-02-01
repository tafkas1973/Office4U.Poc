using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Office4U.Articles.ImportExport.Api.Controllers;
using Office4U.Common;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Presentation.Controller.Articles;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.Tests.Builders;
using Office4U.Tests.TestData;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
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

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IArticleRepository> _articleRepository;

        private Mock<IGetArticlesQuery> _listQueryMock;
        private Mock<IGetArticleQuery> _singleQueryMock;
        private Mock<ICreateArticleCommand> _createCommandMock;
        private Mock<IUpdateArticleCommand> _updateCommandMock;
        private Mock<IDeleteArticleCommand> _deleteCommandMock;
        private PagedList<ArticleDto> _articlesDtoPagedList;
        private Mapper _readMapper;
        private Mapper _writeMapper;

        [SetUp]
        public void Setup()
        {
            _articleParams = new ArticleParams();
            _testArticles = ArticleList.GetShortList().AsEnumerable();
            var articlesPagedList = new PagedList<Article>(items: _testArticles, count: 3, pageNumber: 1, pageSize: 10);

            _readMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ReadApplication.Helpers.AutoMapperProfiles>()));
            _writeMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Office4U.WriteApplication.Helpers.AutoMapperProfiles>()));

            _listQueryMock = new Mock<IGetArticlesQuery>();
            _singleQueryMock = new Mock<IGetArticleQuery>();
            _createCommandMock = new Mock<ICreateArticleCommand>();
            _updateCommandMock = new Mock<IUpdateArticleCommand>();
            _updateCommandMock = new Mock<IUpdateArticleCommand>();
            _deleteCommandMock = new Mock<IDeleteArticleCommand>();

            var articlesDtoList = _readMapper.Map<IEnumerable<ArticleDto>>(articlesPagedList);
            _articlesDtoPagedList = new PagedList<ArticleDto>(articlesDtoList, articlesPagedList.TotalCount, articlesPagedList.CurrentPage, articlesPagedList.PageSize);
            _listQueryMock
                .Setup(m => m.Execute(It.IsAny<ArticleParams>()))
                .Returns(Task.FromResult(_articlesDtoPagedList));

            _unitOfWork = new Mock<IUnitOfWork>();
            _articleRepository = new Mock<IArticleRepository>();
            _unitOfWork.Setup(uow => uow.ArticleRepository).Returns(_articleRepository.Object);

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
        public async Task GetArticle_WithExistingId_ReturnsTheCorrectArticleDto()
        {
            // arrange
            var existingId = 2;
            _singleQueryMock
                .Setup(m => m.Execute(It.IsAny<int>()))
                .Returns(Task.FromResult(_articlesDtoPagedList[existingId - 1]));

            // act
            var result = await _articlesController.GetArticle(existingId);

            // assert
            _singleQueryMock.Verify(m => m.Execute(2), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ActionResult<ArticleDto>)));
            Assert.That(((ObjectResult)result.Result).Value.GetType(), Is.EqualTo(typeof(ArticleDto)));
            Assert.That(((ArticleDto)((ObjectResult)result.Result).Value).Id, Is.EqualTo(2));
        }

        [Test]
        public async Task GetArticle_WithNonExistingId_ReturnsNull()
        {
            // arrange
            var nonExistingId = 99;
            _singleQueryMock
                .Setup(m => m.Execute(It.IsAny<int>()))
                .Returns(Task.FromResult<ArticleDto>(null));

            // act
            var result = await _articlesController.GetArticle(nonExistingId);

            // assert
            _singleQueryMock.Verify(m => m.Execute(nonExistingId), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ActionResult<ArticleDto>)));
            Assert.That(((ObjectResult)result.Result).Value, Is.EqualTo(null));
        }

        [Test]
        public async Task CreateArticle_WithSucceedingCommit_ReturnsStatusCodeNoContent()
        {
            var articleForCreationDto = new ArticleForCreationDto()
            {
                Code = "New Test Article",
                Name1 = "1st article Updated",
                SupplierId = "supId",
                SupplierReference = "supRef",
                PurchasePrice = 99.99M,
                Unit = "ST"
            };
            var newArticle = _writeMapper.Map<Article>(articleForCreationDto);
            var articleForReturnDto = _writeMapper.Map<ArticleForReturnDto>(newArticle);
            _createCommandMock.Setup(m => m.Execute(It.IsAny<ArticleForCreationDto>())).Returns(Task.FromResult(articleForReturnDto));

            // act
            var result = await _articlesController.CreateArticle(articleForCreationDto);

            // assert
            _createCommandMock.Verify(m => m.Execute(It.IsAny<ArticleForCreationDto>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(CreatedAtRouteResult)));
            Assert.That(((CreatedAtRouteResult)result).StatusCode, Is.EqualTo(201));
            Assert.That(((CreatedAtRouteResult)result).RouteName, Is.EqualTo("GetArticle"));
            Assert.That(((CreatedAtRouteResult)result).RouteValues.First(), Is.EqualTo(new KeyValuePair<string, int>("id", articleForReturnDto.Id)));
            Assert.That(((CreatedAtRouteResult)result).Value, Is.EqualTo(articleForReturnDto));
        }

        [Test]
        public async Task CreateArticle_WithFailingCommit_ReturnsStatusBadRequest()
        {
            var articleForCreationDto = new ArticleForCreationDto()
            {
                Code = "New Test Article",
                Name1 = "1st article Updated",
                SupplierId = "supId",
                SupplierReference = "supRef",
                PurchasePrice = 99.99M,
                Unit = "ST"
            };
            var newArticle = _writeMapper.Map<Article>(articleForCreationDto);
            var articleForReturnDto = _writeMapper.Map<ArticleForReturnDto>(newArticle);
            _createCommandMock.Setup(m => m.Execute(It.IsAny<ArticleForCreationDto>())).Returns(Task.FromResult<ArticleForReturnDto>(null));

            // act
            var result = await _articlesController.CreateArticle(articleForCreationDto);

            // assert
            _createCommandMock.Verify(m => m.Execute(It.IsAny<ArticleForCreationDto>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(BadRequestObjectResult)));
            Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(400));
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Failed to create article"));
        }

        [Test]
        public async Task UpdateArticle_WithSucceedingCommit_ReturnsStatusCodeNoContent()
        {
            // arrange
            _articleRepository.Setup(ar => ar.GetArticleByIdAsync(1)).Returns(Task.FromResult(_testArticles.First()));
            _unitOfWork.Setup(uow => uow.Commit()).Returns(Task.FromResult(true));
            _updateCommandMock.Setup(m => m.Execute(It.IsAny<ArticleForUpdateDto>())).Returns(Task.FromResult(true));
            var articleForUpdateDto = new ArticleForUpdateDto() { Id = 1, Name1 = "1st article Updated" };

            // act
            var result = await _articlesController.UpdateArticle(articleForUpdateDto);

            // assert
            _updateCommandMock.Verify(m => m.Execute(It.IsAny<ArticleForUpdateDto>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(NoContentResult)));
            Assert.That(((NoContentResult)result).StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task UpdateArticle_WithFailingCommit_ReturnsStatusBadRequest()
        {
            // arrange
            _articleRepository.Setup(ar => ar.GetArticleByIdAsync(1)).Returns(Task.FromResult(_testArticles.First()));
            _updateCommandMock.Setup(m => m.Execute(It.IsAny<ArticleForUpdateDto>())).Returns(Task.FromResult(false));
            var articleForUpdateDto = new ArticleForUpdateDto() { Id = 5, Name1 = "1st article Updated" };

            // act
            var result = await _articlesController.UpdateArticle(articleForUpdateDto);

            // assert
            _updateCommandMock.Verify(m => m.Execute(It.IsAny<ArticleForUpdateDto>()), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(BadRequestObjectResult)));
            Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(400));
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Failed to update article"));
        }

        [Test]
        public async Task DeleteArticle_WithSucceedingCommit_ReturnsStatusCodeNoContent()
        {
            _deleteCommandMock.Setup(m => m.Execute(It.IsAny<int>())).Returns(Task.FromResult(true));
            var idOfArticleToDelete = 1;

            // act
            var result = await _articlesController.DeleteArticle(idOfArticleToDelete);

            // assert
            _deleteCommandMock.Verify(m => m.Execute(idOfArticleToDelete), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)result).StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteArticle_WithFailingCommit_ReturnsStatusCodeBadRequest()
        {
            _deleteCommandMock.Setup(m => m.Execute(It.IsAny<int>())).Returns(Task.FromResult(false));
            var idOfArticleToDelete = 1;

            // act
            var result = await _articlesController.DeleteArticle(idOfArticleToDelete);

            // assert
            _deleteCommandMock.Verify(m => m.Execute(idOfArticleToDelete), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(BadRequestObjectResult)));
            Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(400));
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Failed to delete article"));
        }
    }
}
