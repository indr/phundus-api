using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    [TestFixture]
    public class UserRepositoryTests : BaseTestFixture
    {
        private IUserRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = Container.Resolve<IUserRepository>();
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
                _repo.Save(user);
                uow.TransactionalFlush();
                id = user.Id;
                Assert.That(user.Id, Is.GreaterThan(0));
            }

            using (UnitOfWork.Start())
            {
                var user = _repo.Get(id);
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
                var user = _repo.Get(1);
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
                var user = _repo.Get(2);
                version = user.Version;
                user.FirstName = "Marshall " + (version + 1);
                _repo.Update(user);
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                var user = _repo.Get(2);
                Assert.That(user.Version, Is.EqualTo(version + 1));    
            }
        }

        [Test]
        public void Can_find_by_email()
        {
            using (UnitOfWork.Start())
            {
                var user = _repo.FindByEmail("marshall.eriksen@example.com");
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(2));
            }
        }

        [Test]
        public void Find_by_email_returns_null()
        {
            using (UnitOfWork.Start())
            {
                var user = _repo.FindByEmail("this.does@not.exist");
                Assert.That(user, Is.Null);
            }
        }
    }
}
