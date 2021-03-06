﻿using AutoMapper;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Office4U.Data.Ef.SqlServer;
using Office4U.Data.Ef.SqlServer.Articles.Repositories;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.UnitOfWork;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Tests.TestData;
using Office4U.WriteApplication.Articles.Commands;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Interfaces.IOC;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.WriteApplication.Tests.Articles.Commands
{
    public class CreateArticleCommandTests : DatabaseFixture
    {
        private Mapper _writeMapper;

        [SetUp]
        public void SetUp()
        {
            _writeMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<Helpers.AutoMapperProfiles>()));
        }

        [Test]
        public async Task Create_ValidObject_ReturnsNewArticle()
        {
            //Arrange
            var unitOfWork = new UnitOfWork(TestContext, new ArticleRepository(TestContext), null);

            var articleForCreation = new ArticleForCreationDto()
            {
                Code = "new code",
                Name1 = "new name",
                SupplierId = "sup id",
                SupplierReference = "sup ref",
                PurchasePrice = 99.99M,
                Unit = "ST",
                PhotoUrl = "www.retail4u.be/newarticle.jpg"
            };
            var createArticleCommand = new CreateArticleCommand(unitOfWork, _writeMapper);
            var articleCountBefore = TestContext.Articles.Count();

            //Act
            var result = await createArticleCommand.Execute(articleForCreation);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(ArticleForReturnDto)));
            Assert.That(TestContext.Articles.Count, Is.EqualTo(articleCountBefore + 1));
            Assert.That(result.Id, Is.Not.Null);
            Assert.That(result.Name1, Is.EqualTo(articleForCreation.Name1));
            Assert.That(result.Photos, Is.Not.Null);
            Assert.That(result.Photos.First(), Is.Not.Null);
            Assert.That(result.Photos.First().Url, Is.EqualTo("www.retail4u.be/newarticle.jpg"));
        }

        [Test]
        public async Task Create_InvalidObject_ReturnsNull()
        {
            //Arrange
            var testArticles = ArticleList.GetDefaultList();
            var articleDbSetMock = testArticles.AsQueryable().BuildMockDbSet();
            var dataContextMock = new Mock<CommandDbContext>();
            dataContextMock.Setup(m => m.Articles).Returns(articleDbSetMock.Object);
            var articleRepository = new Mock<IArticleRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.ArticleRepository).Returns(articleRepository.Object);

            var articleForCreation = new ArticleForCreationDto();
            var createArticleCommand = new CreateArticleCommand(unitOfWorkMock.Object, _writeMapper);
            unitOfWorkMock.Setup(uow => uow.Commit()).Returns(Task.FromResult(false));

            //Act
            var result = await createArticleCommand.Execute(articleForCreation);

            //Assert
            articleRepository.Verify(r => r.Add(It.IsAny<Article>()), Times.Once);
            Assert.That(result, Is.Null);
        }
    }
}
