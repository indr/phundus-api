using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain;
using phiNdus.fundus.Core.Domain.Installers;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.IntegrationTests
{
    [TestFixture]
    public class UserServiceTests
    {
        #region SetUp/TearDown

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(
                new RepositoriesInstaller());
            IoC.Container.Register(Component.For<IUnitOfWorkFactory>().Instance(
                new NHibernateUnitOfWorkFactory(new[] { Assembly.GetAssembly(typeof(BaseEntity)) })));
            IoC.Container.Register(Component.For<IMailGateway>().ImplementedBy(typeof (MailGateway)).LifeStyle.Transient);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            Sut = new UserService();
        }

        #endregion

        private UserService Sut { get; set; }

        [Test]
        public void CreateUser_returns_dto_of_new_user()
        {
            Assert.Ignore("MailGateway-Konfiguration muss noch bewerkstelligt werden.");
            var dto = Sut.CreateUser("stella.zinman@example.com", "1234");
            
            Assert.That(dto.Email, Is.EqualTo("stella.zinman@example.com"));
            Assert.That(dto.Id, Is.GreaterThan(0));
            Assert.That(dto.IsApproved, Is.False);
        }

        [Test]
        public void GetUser_returns_dto()
        {
            var dto = Sut.GetUser("ted.mosby@example.com");
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
        }
    }
}