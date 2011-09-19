using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    
    [TestFixture]
    public class ArticleServiceTests : BaseTestFixture
    {

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new ArticleService();
        }

        protected ArticleService Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new ArticleService();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Is_derived_from_BaseService()
        {
            var sut = new ArticleService();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf(typeof(BaseService)));
        }


        [Test]
        public void GetArticles_calls_repository_FindAll()
        {
            var mockArticleRepository = GenerateAndRegisterMock<IArticleRepository>();
            mockArticleRepository.Expect(x => x.FindAll()).Return(new List<Article>());

            Sut.GetArticles();

            mockArticleRepository.VerifyAllExpectations();
        }
        
        [Test]
        public void GetArticle_calls_repository_Get()
        {
            var mockArticleRepository = GenerateAndRegisterMock<IArticleRepository>();
            mockArticleRepository.Expect(x => x.Get(1));

            Sut.GetArticle(1);

            mockArticleRepository.VerifyAllExpectations();
        }

        [Test]
        public void CreateArticle_stores_article_in_repository()
        {
            var mockArticleRepository = GenerateAndRegisterMock<IArticleRepository>();
            mockArticleRepository.Expect(x => x.Save(Arg<Article>.Is.Anything));

            Sut.CreateArticle();

            mockArticleRepository.VerifyAllExpectations();
        }

        [Test]
        public void CreateArticle_flushes_transaction()
        {
            GenerateAndRegisterStub<IArticleRepository>();
            var mockUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            mockUnitOfWork.Expect(x => x.TransactionalFlush());

            Sut.CreateArticle();

            mockUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_stores_article_in_repository()
        {
            var mockArticleRepository = GenerateAndRegisterMock<IArticleRepository>();
            mockArticleRepository.Expect(x => x.Save(Arg<Article>.Is.Anything));

            Sut.UpdateArticle();

            mockArticleRepository.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_flushes_transaction()
        {
            GenerateAndRegisterStub<IArticleRepository>();
            var mockUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            mockUnitOfWork.Expect(x => x.TransactionalFlush());

            Sut.UpdateArticle();

            mockUnitOfWork.VerifyAllExpectations();
        }
    }
}
