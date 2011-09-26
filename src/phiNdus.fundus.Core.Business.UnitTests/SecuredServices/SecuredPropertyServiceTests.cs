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
    public class SecuredPropertyServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            GenerateAndRegisterStubUnitOfWork();

            Sut = new SecuredPropertyService();
        }

        #endregion

        private PropertyService FakePropertyService { get; set; }
        private IUserRepository FakeUserRepo { get; set; }

        protected User SessionAdmin { get; set; }
        protected User SessionUser { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<PropertyService>() == null)
                FakePropertyService = GenerateAndRegisterStub<PropertyService>();
            if (IoC.TryResolve<IUserRepository>() == null)
                FakeUserRepo = GenerateAndRegisterStub<IUserRepository>();

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

        private IPropertyService Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new SecuredPropertyService();
            Assert.That(sut, Is.Not.Null);
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

        [Test]
        public void GetProperties_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var dtos = new PropertyDto[0];
            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.GetProperties()).Return(dtos);
            var actual = Sut.GetProperties("valid");

            Assert.That(actual, Is.SameAs(dtos));
        }

        [Test]
        public void GetProperties_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetProperties("invalid"));
        }

        [Test]
        public void GetProperties_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetProperties(null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }
    }
}