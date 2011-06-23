using NHibernate.Exceptions;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IUserRepository>();

            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                // TODO: Immer alle löschen...
                UnitOfWork.CurrentSession.Delete("from Membership m where m.Email = 'john.doe@example.com'");
                uow.TransactionalFlush();
            }
        }

        #endregion

        private IUserRepository Sut { get; set; }

        private static User CreateUser()
        {
            var result = new User();
            result.Role = new RoleRepository().Get(1);
            result.FirstName = "John";
            result.LastName = "Doe";
            result.Membership.Email = "john.doe@example.com";
            result.Membership.GenerateValidationKey();
            return result;
        }

        [Test]
        public void CanFindByEmail()
        {
            int id;
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = CreateUser();
                user.Membership.Email = "john.doe@example.com";
                id = Sut.Save(user).Id;
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                User user = Sut.FindByEmail("john.doe@example.com");
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(id));
                Assert.That(user.Membership.Email, Is.EqualTo("john.doe@example.com"));
            }
        }

        [Test]
        public void CanSaveAndGetById()
        {
            object id;
            string validationKey;
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = CreateUser();
                validationKey = user.Membership.ValidationKey;
                id = Sut.Save(user).Id;
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                User user = Sut.Get(id);
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(id));
                Assert.That(user.FirstName, Is.EqualTo("John"));
                Assert.That(user.LastName, Is.EqualTo("Doe"));
                Assert.That(user.Membership.ValidationKey, Is.EqualTo(validationKey));
            }
        }

        [Test]
        public void Can_find_by_SessionKey()
        {
            using (UnitOfWork.Start())
            {
                User user = Sut.FindBySessionKey("0eb6182f7d75197bafc6");
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(2));
            }
        }

        [Test]
        public void FindByValidationKeyReturnsNull()
        {
            using (UnitOfWork.Start())
            {
                User user = Sut.FindByValidationKey("not.existing.key");
                Assert.That(user, Is.Null);
            }
        }

        [Test]
        public void FindByValidationKeyReturnsUser()
        {
            int id;
            string validationKey;
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = CreateUser();
                validationKey = user.Membership.GenerateValidationKey();
                id = Sut.Save(user).Id;
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                User user = Sut.FindByValidationKey(validationKey);
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(id));
                Assert.That(user.Membership.ValidationKey, Is.EqualTo(validationKey));
            }
        }

        [Test]
        public void Find_by_email_returns_null()
        {
            using (UnitOfWork.Start())
            {
                User user = Sut.FindByEmail("this.does@not.exist");
                Assert.That(user, Is.Null);
            }
        }

        [Test]
        public void SaveWithAlreadyExistingEmailThrows()
        {
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user1 = CreateUser();
                user1.Membership.Email = "john.doe@example.com";
                Sut.Save(user1);

                User user2 = CreateUser();
                user2.Membership.Email = "john.doe@example.com";
                Sut.Save(user2);
                var ex = Assert.Throws<GenericADOException>(uow.TransactionalFlush);
                Assert.That(ex.InnerException.Message,
                            // TODO: Etwas besseres finden...
                            Is.StringContaining("Cannot insert duplicate key in object 'dbo.Membership'")
                            .Or.StringContaining("Ein doppelter Schlüssel kann in das 'dbo.Membership'-Objekt nicht eingefügt werden"));
            }
        }

        [Test]
        public void SaveWithNullEmailThrows()
        {
            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = CreateUser();
                user.Membership.Email = null;
                Sut.Save(user);
                var ex = Assert.Throws<GenericADOException>(uow.TransactionalFlush);
                Assert.That(ex.InnerException.Message,
                            // TODO: Etwas besseres finden...
                            Is.StringContaining("Cannot insert the value NULL into column 'Email'")
                            .Or.StringContaining("Der Wert NULL kann in die 'Email'-Spalte"));
            }
        }

        [Test]
        public void Save_inserts_new_user_with_membership()
        {
            int id = 0;
            using (IUnitOfWork uow = UnitOfWork.Start())
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
                User user = Sut.Get(id);
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
            int version = 0;

            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                User user = Sut.Get(2);
                version = user.Version;
                user.FirstName = "Marshall " + (version + 1);
                Sut.Update(user);
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                User user = Sut.Get(2);
                Assert.That(user.Version, Is.EqualTo(version + 1));
            }
        }
    }
}