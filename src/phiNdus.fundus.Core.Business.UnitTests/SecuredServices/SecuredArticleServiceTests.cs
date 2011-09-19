using System;
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

        private ArticleService FakeArticleService { get; set; }
        private IUserRepository FakeUserRepo { get; set; }

        protected User SessionUser { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<ArticleService>() == null)
                FakeArticleService = GenerateAndRegisterStub<ArticleService>();
            if (IoC.TryResolve<IUserRepository>() == null)
                FakeUserRepo = GenerateAndRegisterStub<IUserRepository>();

            if (SessionUser == null)
                SessionUser = new User();
        }

        private IArticleService Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new SecuredArticleService();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void GetArticle_calls_GetArticle()
        {
            FakeArticleService = GenerateAndRegisterStrictMock<ArticleService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionUser);
            FakeArticleService.Expect(x => x.GetArticle(1)).Return(null);
            Sut.GetArticle("valid", 1);

            FakeArticleService.VerifyAllExpectations();
        }

        [Test]
        public void GetArticle_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionUser);
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
        public void Is_derived_from_BaseSecuredService()
        {
            Assert.That(Sut, Is.InstanceOf(typeof (BaseSecuredService)));
        }
    }
}