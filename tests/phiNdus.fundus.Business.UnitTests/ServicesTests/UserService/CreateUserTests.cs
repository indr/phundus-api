using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Business.UnitTests.Services;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Domain.Settings;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.ServicesTests.UserService
{
    [TestFixture]
    public class CreateUserTests : UnitTestBase<Business.Services.UserService>
    {
        [SetUp]
        public override void SetUp()
        {
            Assert.Ignore("TODO");

            base.SetUp();

            Obsolete_MockFactory = null;

            MockUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            MockUserRepository = GenerateAndRegisterMock<IUserRepository>();
            MockRoleRepository = GenerateAndRegisterMock<IRoleRepository>();
            MockMailGateway = GenerateAndRegisterMock<IMailGateway>();

            StubSettings = MockRepository.GenerateMock<ISettings>();
            Settings.SetGlobalNonThreadSafeSettings(StubSettings);
            StubSettings.Stub(x => x.Mail).Return(MockRepository.GenerateMock<IMailSettings>());
            StubSettings.Stub(x => x.Common).Return(MockRepository.GenerateMock<ICommonSettings>());
            StubSettings.Common.Stub(x => x.ServerUrl).Return("fundus.example.com");
            StubSettings.Mail.Stub(x => x.Smtp).Return(MockRepository.GenerateMock<ISmtpSettings>());
            StubSettings.Mail.Stub(x => x.Templates).Return(MockRepository.GenerateMock<IMailTemplatesSettings>());
            StubSettings.Mail.Templates.Stub(x => x.UserAccountValidation).Return(MockRepository.GenerateMock<IMailTemplateSettings>());
            StubSettings.Mail.Templates.UserAccountValidation.Stub(x => x.Subject).Return("");
            StubSettings.Mail.Templates.UserAccountValidation.Stub(x => x.TextBody).Return("");

            Sut = new Business.Services.UserService();

            MockRoleRepository.Stub(x => x.Get(1)).Return(new Role());
            MockUserRepository.Stub(x => x.Save(Arg<User>.Is.Anything));
            MockMailGateway.Stub(x => x.Send(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything));
        }

        protected ISettings StubSettings { get; set; }

        protected IMailGateway MockMailGateway { get; set; }

        protected IRoleRepository MockRoleRepository { get; set; }

        protected IUserRepository MockUserRepository { get; set; }

        [Test]
        public void CreateUserFlushesTransactionAndDisposesUnitOfWork()
        {
            MockUserRepository.Stub(x => x.FindByEmail(Arg<string>.Is.Anything)).Return(null);

            Sut.CreateUser("john@example.com", "1234", "", "", 1);

            MockUnitOfWork.AssertWasCalled(x => x.TransactionalFlush());
            MockUnitOfWork.AssertWasCalled(x => x.Dispose());
        }

        [Test]
        public void CreateUserWithEmptyEmailThrows()
        {
            Assert.Ignore("TODO");
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.CreateUser("", "", "", "", 1));
        }

        [Test]
        public void CreateUserLowersEmail()
        {
            MockUserRepository.Stub(x => x.FindByEmail(Arg<string>.Is.Anything)).Return(null);

            var dto = Sut.CreateUser("Ted.Mosby@example.com", "", "", "", 1);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
        }

        [Test]
        public void CreateUserReturnsDto()
        {
            MockUserRepository.Stub(x => x.FindByEmail(Arg<string>.Is.Anything)).Return(null);

            var dto = Sut.CreateUser("ted.mosby@example.com", "", "Ted", "Mosby", 123456);

            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
            Assert.That(dto.FirstName, Is.EqualTo("Ted"));
            Assert.That(dto.LastName, Is.EqualTo("Mosby"));
            Assert.That(dto.JsNumber, Is.EqualTo(123456));
        }

        [Test]
        public void CreateUserSaveNewUserToRepository()
        {
            MockUserRepository.Stub(x => x.FindByEmail(Arg<string>.Is.Anything)).Return(null);

            Sut.CreateUser("ted.mosby@example.com", "", "", "", 1);

            MockUserRepository.AssertWasCalled(x => x.Save(Arg<User>.Is.Anything));
        }

        [Test]
        public void CreateUserSendsEmailWithValidationLink()
        {
            StubSettings.Mail.Templates.UserAccountValidation.ClearBehavior();
            StubSettings.Mail.Templates.UserAccountValidation.Stub(x => x.Subject).Return("[fundus] User Account Validation");
            StubSettings.Mail.Templates.UserAccountValidation.Stub(x => x.TextBody).Return(@"Hello [User.FirstName]

Link: [Link.UserAccountValidation]
");
            MockUserRepository.Stub(x => x.FindByEmail(Arg<string>.Is.Anything)).Return(null);

            Sut.CreateUser("ted.mosby@example.com", "password", "Ted", "", 1);

            MockMailGateway.AssertWasCalled(x => x.Send(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything));

            MockMailGateway.AssertWasCalled(x => x.Send(
                Arg<string>.Is.Equal("ted.mosby@example.com"),
                Arg<string>.Is.Same("[fundus] User Account Validation"),
                Arg<string>.Matches(y => y.StartsWith("Hello Ted") && y.Contains(@"http://fundus.example.com/Account/Validation/")
                        && new Regex(@"/[\w]{24}").Match(y).Success)));
        }

        [Test]
        public void CreateUserWhenEmailAlreadyTakenThrows()
        {
            MockUserRepository.Expect(x => x.FindByEmail("ted.mosby@example.com")).Return(new User());

            Assert.Throws<EmailAlreadyTakenException>(() => Sut.CreateUser("ted.mosby@example.com", "", "", "", 1));
        }
    }
}
