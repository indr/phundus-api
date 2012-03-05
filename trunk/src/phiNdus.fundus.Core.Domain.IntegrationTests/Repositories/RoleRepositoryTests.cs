using NHibernate.Exceptions;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class RoleRepositoryTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IRoleRepository>();
        }

        #endregion

        protected IRoleRepository Sut { get; set; }

        [Test]
        public void Delete_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                fromRepo.Name = fromRepo.Name + " updated";
                Sut.Delete(fromRepo);
                Assert.Throws<GenericADOException>(() => uow.TransactionalFlush());
            }
        }

        [Test]
        public void FindAll_returns_predefined_roles()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.FindAll();
                Assert.That(fromRepo.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void Get_1_returns_User_role()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Name, Is.EqualTo("User"));
            }
        }

        [Test]
        public void Get_2_returns_Administrator_role()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(2);
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Name, Is.EqualTo("Admin"));
            }
        }


        [Test]
        public void Save_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var role = new Role();
                role.Name = "New Role";
                Assert.Throws<GenericADOException>(() => Sut.Save(role));
                // Bei HiLo:
                //Assert.Throws<GenericADOException>(() => uow.TransactionalFlush());
            }
        }

        [Test]
        public void Update_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                fromRepo.Name = fromRepo.Name + " updated";
                Sut.Update(fromRepo);
                Assert.Throws<GenericADOException>(() => uow.TransactionalFlush());
            }
        }

        [Test]
        public void User_role_from_repo_equals_static_domain_entity()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo, Is.EqualTo(Role.User));
            }
        }
    }
}