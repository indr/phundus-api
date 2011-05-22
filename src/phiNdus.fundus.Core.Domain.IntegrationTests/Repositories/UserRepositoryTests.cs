using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    internal class UserRepositoryTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IUserRepository>();
        }

        #endregion

        private IUserRepository Sut { get; set; }

        [Test]
        public void Can_find_by_SessionKey()
        {
            using (UnitOfWork.Start())
            {
                var user = Sut.FindBySessionKey("0eb6182f7d75197bafc6");
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(2));
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
                Assert.That(user.Membership.Password, Is.EqualTo("0a2a5ff03fc204833be2e4e1b6ec2dd2"));
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
        public void Find_by_email_returns_null()
        {
            using (UnitOfWork.Start())
            {
                var user = Sut.FindByEmail("this.does@not.exist");
                Assert.That(user, Is.Null);
            }
        }

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
                // TODO,Inder: Session-API-Model mit detached Objects nicht verstanden? =)
                UnitOfWork.CurrentSession.Refresh(user.Role);
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
    }
}