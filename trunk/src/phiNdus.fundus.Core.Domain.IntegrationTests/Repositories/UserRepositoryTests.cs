using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Bootstrapper;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(
                new RepositoriesInstaller());
            IoC.Container.Register(Component.For<IUnitOfWorkFactory>().Instance(
                new NHibernateUnitOfWorkFactory(new[] { Assembly.GetAssembly(typeof(BaseEntity)) })));
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Container.Resolve<IUserRepository>();
        }

        private IUserRepository Sut { get; set; }

        [Test]
        public void Save_inserts_new_user_with_membership()
        {
            var id = 0;
            using (var uow = UnitOfWork.Start())
            {
                var user = new User();
                user.FirstName = "Lily";
                user.LastName = "Aldrin";
                user.Membership.Email = "lily.aldrin@example.com";
                Sut.Save(user);
                uow.TransactionalFlush();
                id = user.Id;
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            using (UnitOfWork.Start())
            {
                var user = Sut.Get(id);
                Assert.That(user, Is.Not.Null);
                Assert.That(user.FirstName, Is.EqualTo("Lily"));
                Assert.That(user.LastName, Is.EqualTo("Aldrin"));
                Assert.That(user.Membership, Is.Not.Null);
                Assert.That(user.Membership.Email, Is.EqualTo("lily.aldrin@example.com"));
            }
        }

        [Test]
        public void Can_get_user_by_id()
        {
            using (UnitOfWork.Start())
            {
                var user = Sut.Get(1);
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(1));
                Assert.That(user.FirstName, Is.EqualTo("Ted"));
                Assert.That(user.LastName, Is.EqualTo("Mosby"));
            }
        }

        [Test]
        public void Update_user_increments_version()
        {
            var version = 0;
            
            using (var uow = UnitOfWork.Start())
            {
                var user = Sut.Get(2);
                version = user.Version;
                user.FirstName = "Marshall " + (version + 1);
                Sut.Update(user);
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                var user = Sut.Get(2);
                Assert.That(user.Version, Is.EqualTo(version + 1));    
            }
        }

        [Test]
        public void Can_find_by_email()
        {
            using (UnitOfWork.Start())
            {
                var user = Sut.FindByEmail("marshall.eriksen@example.com");
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(2));
            }
        }

        [Test]
        public void Find_by_email_returns_null()
        {
            using (UnitOfWork.Start())
            {
                var user = Sut.FindByEmail("this.does@not.exist");
                Assert.That(user, Is.Null);
            }
        }
    }
}
