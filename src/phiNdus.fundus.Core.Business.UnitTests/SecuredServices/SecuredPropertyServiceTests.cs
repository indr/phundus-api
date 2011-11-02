﻿using System;
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

            Sut = new SecuredFieldsService();
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

        private IFieldsService Sut { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new SecuredFieldsService();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void CreateProperty_calls_CreateProperty()
        {
            FakePropertyService = GenerateAndRegisterStrictMock<PropertyService>();
            GenerateAndRegisterMissingStubs();

            var dto = new FieldDefinitionDto();
            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.CreateProperty(dto)).Return(1);
            Sut.CreateField("valid", dto);

            FakePropertyService.VerifyAllExpectations();
        }

        [Test]
        public void CreateProperty_returns_id()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.CreateProperty(Arg<FieldDefinitionDto>.Is.Anything)).Return(1);

            var actual = Sut.CreateField("valid", new FieldDefinitionDto());

            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void CreateProperty_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();
            Assert.Throws<InvalidSessionKeyException>(() => Sut.CreateField("invalid", null));
        }

        [Test]
        public void CreateProperty_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.CreateField(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void CreateProperty_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionUser);

            Assert.Throws<AuthorizationException>(() => Sut.CreateField("valid", new FieldDefinitionDto()));
        }

        [Test]
        public void DeleteProperty_calls_DeleteProperty()
        {
            FakePropertyService = GenerateAndRegisterStrictMock<PropertyService>();
            GenerateAndRegisterMissingStubs();

            var dto = new FieldDefinitionDto();
            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.DeleteProperty(dto));
            Sut.DeleteField("valid", dto);

            FakePropertyService.VerifyAllExpectations();
        }

        [Test]
        public void DeleteProperty_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();
            Assert.Throws<InvalidSessionKeyException>(() => Sut.DeleteField("invalid", null));
        }

        [Test]
        public void DeleteProperty_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.DeleteField(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void DeleteProperty_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionUser);

            Assert.Throws<AuthorizationException>(() => Sut.DeleteField("valid", new FieldDefinitionDto()));
        }

        [Test]
        public void GetProperty_calls_GetProperty()
        {
            FakePropertyService = GenerateAndRegisterStrictMock<PropertyService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.GetProperty(1)).Return(null);
            Sut.GetField("valid", 1);

            FakePropertyService.VerifyAllExpectations();
        }

        [Test]
        public void GetProperty_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            var dto = new FieldDefinitionDto();
            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.GetProperty(1)).Return(dto);
            var actual = Sut.GetField("valid", 1);

            Assert.That(actual, Is.SameAs(dto));
        }

        [Test]
        public void GetProperty_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetField("invalid", 1));
        }

        [Test]
        public void GetProperty_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetField(null, 1));
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
        public void UpdateProperty_calls_UpdateArticle()
        {
            FakePropertyService = GenerateAndRegisterStrictMock<PropertyService>();
            GenerateAndRegisterMissingStubs();

            var dto = new FieldDefinitionDto();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakePropertyService.Expect(x => x.UpdateProperty(Arg<FieldDefinitionDto>.Is.Equal(dto)));
            Sut.UpdateField("valid", dto);

            FakePropertyService.VerifyAllExpectations();
        }

        [Test]
        public void UpdateProperty_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.UpdateField("invalid", null));
        }

        [Test]
        public void UpdateProperty_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateField(null, null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void UpdateProperty_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();
            FakeUserRepo.Stub(x => x.FindBySessionKey("valid")).Return(SessionUser);

            Assert.Throws<AuthorizationException>(() => Sut.UpdateField("valid", null));
        }
    }
}