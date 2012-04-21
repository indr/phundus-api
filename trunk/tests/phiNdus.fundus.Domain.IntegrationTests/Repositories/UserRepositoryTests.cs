using NHibernate.Exceptions;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.Builders;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests : DomainComponentTestBase<IUserRepository>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = IoC.Resolve<IUserRepository>();

            using (IUnitOfWork uow = UnitOfWork.Start())
            {
                // TODO: Immer alle löschen...
                UnitOfWork.CurrentSession.Delete("from Membership m where m.Email = 'john.doe@example.com'");
                uow.TransactionalFlush();
            }
        }

        #endregion

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
            // Arrange
            int userId;
            using (var uow = UnitOfWork.Start())
            {
                var user = new UserBuilder().Build();
                user.Membership.LogOn("abcd", "1234");
                userId = user.Id;
                uow.TransactionalFlush();
            }

            // Assert
            using (UnitOfWork.Start())
            {
                User user = Sut.FindBySessionKey("abcd");
                Assert.That(user, Is.Not.Null);
                Assert.That(user.Id, Is.EqualTo(userId));
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
                            .Or.StringContaining("Ein doppelter Schlüssel kann in das 'dbo.Membership'-Objekt nicht eingefügt werden")
                            .Or.StringContaining("Cannot insert duplicate key row in object 'dbo.Membership' with unique index")
                            .Or.StringContaining("Eine Zeile mit doppeltem Schlüssel kann in das 'dbo.Membership'-Objekt mit dem eindeutigen"));
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
        public void Update_user_increments_version()
        {
            // Arrange
            var userId = 0;
            var version = 0;
            using (var uow = UnitOfWork.Start())
            {
                userId = new UserBuilder().Build().Id;
                uow.TransactionalFlush();
            }

            // Act
            using (var uow = UnitOfWork.Start())
            {
                var user = Sut.Get(userId);
                version = user.Version;
                user.FirstName = "FirstName " + (version + 1);
                Sut.Update(user);
                uow.TransactionalFlush();
            }

            // Assert
            using (UnitOfWork.Start())
            {
                var user = Sut.Get(userId);
                Assert.That(user.Version, Is.EqualTo(version + 1));
            }
        }
    }
}