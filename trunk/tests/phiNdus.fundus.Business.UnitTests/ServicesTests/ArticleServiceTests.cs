using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.Builders;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.ServicesTests
{
    [TestFixture]
    public class ArticleServiceTests : UnitTestBase<ArticleService>
    {
        private Organization _organization;

        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _organization = new Organization(1001);
            FakeUnitOfWork = GenerateAndRegisterStubUnitOfWork();

            Sut = new ArticleService();
            Sut.SecurityContext = SecurityContext(1, "key");
            

            Article = new Article(1, 2);
            Article.Organization = _organization;
        }

        #endregion

        protected SecurityContext SecurityContext(int userId, string key)
        {
            var result = new SecurityContext();
            var user = new User(userId);
           
            user.SelectedOrganization = _organization;
            result.SecuritySession = new SecuritySessionBuilder(user, key).Build();
            return result;
        }

        protected IUnitOfWork FakeUnitOfWork { get; set; }

        protected IArticleRepository FakeArticleRepo { get; set; }
        protected IFieldDefinitionRepository FakePropertyDefRepo { get; set; }

        protected Article Article { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IArticleRepository>() == null)
            {
                FakeArticleRepo = GenerateAndRegisterStub<IArticleRepository>();
                FakeArticleRepo.Expect(x => x.Get(Article.Id)).Return(Article);
                FakeArticleRepo.Expect(x => x.Save(Arg<Article>.Is.Anything)).Return(Article);
            }
            if (IoC.TryResolve<IFieldDefinitionRepository>() == null)
            {
                FakePropertyDefRepo = GenerateAndRegisterStub<IFieldDefinitionRepository>();
                FakePropertyDefRepo.Expect(x => x.Get(FieldDefinition.CreateDateId)).Return(
                    new FieldDefinition(FieldDefinition.CreateDateId, "Erfassungsdatum", DataType.DateTime));
            }
        }

        [Test]
        public void Can_create()
        {
            var sut = new ArticleService();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void CreateArticle_flushes_transaction()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();

            FakeUnitOfWork.Expect(x => x.TransactionalFlush());
            Sut.CreateArticle(new ArticleDto());

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void CreateArticle_returns_id()
        {
            GenerateAndRegisterMissingStubs();

            FakeArticleRepo.Expect(x => x.Save(Arg<Article>.Is.Anything)).Return(new Article(1, 1));
            var actual = Sut.CreateArticle(new ArticleDto());

            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void CreateArticle_stores_article_in_repository()
        {
            FakeArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            FakeArticleRepo.Expect(x => x.Save(Arg<Article>.Is.NotNull));
            Sut.CreateArticle(new ArticleDto());

            FakeArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void CreateArticle_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.CreateArticle(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void GetArticle_calls_repository_Get()
        {
            FakeArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            FakeArticleRepo.Expect(x => x.Get(1)).Return(null);
            Sut.GetArticle(1);

            FakeArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetArticle_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            var actual = Sut.GetArticle(1);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.EqualTo(1));
            Assert.That(actual.Version, Is.EqualTo(2));
        }

        [Test]
        public void GetArticle_with_invalid_id_returns_null()
        {
            GenerateAndRegisterMissingStubs();

            Assert.That(Sut.GetArticle(101), Is.Null);
        }

        [Test]
        public void GetArticles_calls_repository_FindAll()
        {
            FakeArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            FakeArticleRepo.Expect(x => x.FindAll(_organization)).Return(new List<Article>());
            Sut.GetArticles();

            FakeArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetArticles_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var articles = new List<Article>();
            articles.Add(new Article());
            articles.Add(new Article());
            FakeArticleRepo.Expect(x => x.FindAll(_organization)).Return(articles);
            var dtos = Sut.GetArticles();

            Assert.That(dtos, Is.Not.Null);
            Assert.That(dtos, Has.Length.EqualTo(2));
        }

        [Test]
        public void Is_derived_from_BaseService()
        {
            var sut = new ArticleService();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf(typeof (BaseService)));
        }

        [Test]
        public void UpdateArticle_flushes_transaction()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();

            FakeUnitOfWork.Expect(x => x.TransactionalFlush());
            Sut.UpdateArticle(new ArticleDto {Id = 1, Version = 2});

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_stores_article_in_repository()
        {
            FakeArticleRepo = GenerateAndRegisterMock<IArticleRepository>();
            GenerateAndRegisterMissingStubs();

            FakeArticleRepo.Stub(x => x.Get(1)).Return(Article);
            FakeArticleRepo.Expect(x => x.Save(Arg<Article>.Is.Equal(Article))).Return(Article);
            Sut.UpdateArticle(new ArticleDto {Id = 1, Version = 2});

            FakeArticleRepo.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateArticle(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }
    }
}