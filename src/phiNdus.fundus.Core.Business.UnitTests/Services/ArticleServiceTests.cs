using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
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

            FakeUnitOfWork = GenerateAndRegisterStubUnitOfWork();

            Sut = new ArticleService();
        }

        protected ArticleService Sut { get; set; }

        protected IUnitOfWork FakeUnitOfWork { get; set; }

        protected IArticleRepository ArticleRepo { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IArticleRepository>() == null)
                ArticleRepo = GenerateAndRegisterStub<IArticleRepository>();
        }

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
            ArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            ArticleRepo.Expect(x => x.FindAll()).Return(new List<Article>());
            Sut.GetArticles();

            ArticleRepo.VerifyAllExpectations();
        }
        
        [Test]
        public void GetArticle_calls_repository_Get()
        {
            ArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();
            
            ArticleRepo.Expect(x => x.Get(1)).Return(null);
            Sut.GetArticle(1);

            ArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetArticle_with_invalid_id_returns_null()
        {
            GenerateAndRegisterMissingStubs();

            Assert.That(Sut.GetArticle(1), Is.Null);
        }

        [Test]
        public void GetArticle_returns_dto()
        {
            GenerateAndRegisterMissingStubs();
            
            ArticleRepo.Expect(x => x.Get(1)).Return(new Article(1, 2));
            ArticleDto actual = Sut.GetArticle(1);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.EqualTo(1));
            Assert.That(actual.Version, Is.EqualTo(2));
        }

        [Test]
        public void CreateArticle_stores_article_in_repository()
        {
            ArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            ArticleRepo.Expect(x => x.Save(Arg<Article>.Is.Anything));
            Sut.CreateArticle();

            ArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void CreateArticle_flushes_transaction()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();
            
            FakeUnitOfWork.Expect(x => x.TransactionalFlush());
            Sut.CreateArticle();

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_stores_article_in_repository()
        {
            ArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            ArticleRepo.Expect(x => x.Save(Arg<Article>.Is.Anything));
            Sut.UpdateArticle();

            ArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_flushes_transaction()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();

            FakeUnitOfWork.Expect(x => x.TransactionalFlush());
            Sut.UpdateArticle();

            FakeUnitOfWork.VerifyAllExpectations();
        }
    }
}
