using NHibernate.Exceptions;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class RoleRepositoryTests : DomainComponentTestBase<IRoleRepository>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = IoC.Resolve<IRoleRepository>();
        }

        #endregion

        [Test]
        public void Delete_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                Assert.That(fromRepo, Is.Not.Null);
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
                Assert.That(fromRepo.Count, Is.EqualTo(3));
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
        public void Get_3_returns_Administrator_role()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(3);
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Name, Is.EqualTo("Admin"));
            }
        }

        [Test]
        public void Get_2_returns_Chief_role()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(2);
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Name, Is.EqualTo("Chief"));
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