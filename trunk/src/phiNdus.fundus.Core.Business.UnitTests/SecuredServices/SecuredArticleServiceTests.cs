using System;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.SecuredServices
{
    [TestFixture]
    public class SecuredArticleServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            GenerateAndRegisterStubUnitOfWork();

            Sut = new SecuredArticleService();
        }

        #endregion

        private IArticleService Sut { get; set; }

        private ArticleService FakeArticleService { get; set; }
        private IUserRepository FakeUserRepo { get; set; }

        protected User SessionAdmin { get; set; }
        protected User SessionUser { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<ArticleService>() == null)
                FakeArticleService = GenerateAndRegisterStub<ArticleService>();
            if (IoC.TryResolve<PropertyService>() == null)
                FakePropertyService = GenerateAndRegisterStub<PropertyService>();
            if (IoC.TryResolve<IUserRepository>() == null)
                FakeUserRepo = GenerateAndRegisterStub<IUserRepository>();
            if (IoC.TryResolve<IFieldsService>() == null)
                IoC.Container.Register(Component.For<IFieldsService>().ImplementedBy(typeof (SecuredFieldsService)));

            if (SessionAdmin == null)
            {
                SessionAdmin = new User();
                SessionAdmin.Role = Role.Administrator;
            }
            if (SessionUser == null)
            {
                SessionUser = new User();
                SessionUser.Role = Role.User;
            }
        }

        [Test]
        public void Can_create()
        {
            var sut = new SecuredArticleService();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void CreateArticle_calls_CreateArticle()
        {
            FakeArticleService = GenerateAndRegisterStrictMock<ArticleService>();
            GenerateAndRegisterMissingStubs();

            var dto = new ArticleDto();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.CreateArticle(Arg<ArticleDto>.Is.Equal(dto))).Return(1);
            Sut.CreateArticle("valid", dto);

            FakeArticleService.VerifyAllExpectations();
        }

        [Test]
        public void CreateArticle_returns_id()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.CreateArticle(Arg<ArticleDto>.Is.Anything)).Return(1);

            var actual = Sut.CreateArticle("valid", new ArticleDto());

            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void CreateArticle_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();
            Assert.Throws<InvalidSessionKeyException>(() => Sut.CreateArticle("invalid", null));
        }

        [Test]
        public void CreateArticle_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.CreateArticle(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void CreateArticle_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionUser);

            Assert.Throws<AuthorizationException>(() => Sut.CreateArticle("valid", new ArticleDto()));
        }

        [Test]
        public void GetArticle_calls_GetArticle()
        {
            FakeArticleService = GenerateAndRegisterStrictMock<ArticleService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.GetArticle(1)).Return(null);
            Sut.GetArticle("valid", 1);

            FakeArticleService.VerifyAllExpectations();
        }

        [Test]
        public void GetArticle_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.GetArticle(1)).Return(new ArticleDto());
            var actual = Sut.GetArticle("valid", 1);

            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void GetArticle_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetArticle("invalid", 0));
        }

        [Test]
        public void GetArticle_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetArticle(null, 0));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetArticles_calls_GetArticles()
        {
            FakeArticleService = GenerateAndRegisterStrictMock<ArticleService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.GetArticles()).Return(null);
            Sut.GetArticles("valid");

            FakeArticleService.VerifyAllExpectations();
        }

        [Test]
        public void GetArticles_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var dtos = new ArticleDto[0];
            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.GetArticles()).Return(dtos);
            var actual = Sut.GetArticles("valid");

            Assert.That(actual, Is.SameAs(dtos));
        }

        [Test]
        public void GetArticles_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetArticles("invalid"));
        }

        [Test]
        public void GetArticles_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetArticles(null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetProperties_calls_GetProperties()
        {
            FakePropertyService = GenerateAndRegisterStrictMock<PropertyService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.GetProperties()).Return(null);
            Sut.GetProperties("valid");

            FakePropertyService.VerifyAllExpectations();
        }

        protected PropertyService FakePropertyService { get; set; }

        [Test]
        public void GetProperties_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var dtos = new FieldDefinitionDto[0];
            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.GetProperties()).Return(dtos);
            var actual = Sut.GetProperties("valid");

            Assert.That(actual, Is.SameAs(dtos));
        }

        [Test]
        public void GetProperties_with_invalid_sessionKey_does_not_throw()
        {
            GenerateAndRegisterMissingStubs();

            Assert.DoesNotThrow(() => Sut.GetProperties("invalid"));
        }

        [Test]
        public void GetProperties_with_sessionKey_null_does_not_throw()
        {
            GenerateAndRegisterMissingStubs();

            Assert.DoesNotThrow(() => Sut.GetProperties(null));
        }

        [Test]
        public void UpdateArticle_calls_UpdateArticle()
        {
            FakeArticleService = GenerateAndRegisterStrictMock<ArticleService>();
            GenerateAndRegisterMissingStubs();

            var dto = new ArticleDto();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeArticleService.Expect(x => x.UpdateArticle(Arg<ArticleDto>.Is.Equal(dto)));
            Sut.UpdateArticle("valid", dto);

            FakeArticleService.VerifyAllExpectations();
        }

        [Test]
        public void UpdateArticle_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.UpdateArticle("invalid", null));
        }

        [Test]
        public void UpdateArticle_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateArticle(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void UpdateArticle_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionUser);

            Assert.Throws<AuthorizationException>(() => Sut.UpdateArticle("valid", null));
        }
    }
}