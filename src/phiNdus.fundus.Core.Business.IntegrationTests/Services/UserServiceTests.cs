using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new UserService();
        }

        #endregion

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new Installer());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }

        private UserService Sut { get; set; }

        [Test]
        public void CreateUser_returns_dto_of_new_user()
        {
            Assert.Ignore("MailGateway-Konfiguration muss noch bewerkstelligt werden.");
            UserDto dto = Sut.CreateUser("stella.zinman@example.com", "1234");

            Assert.That(dto.Email, Is.EqualTo("stella.zinman@example.com"));
            Assert.That(dto.Id, Is.GreaterThan(0));
            Assert.That(dto.IsApproved, Is.False);
        }

        [Test]
        public void GetUser_returns_dto()
        {
            UserDto dto = Sut.GetUser("ted.mosby@example.com");
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
        }
    }
}