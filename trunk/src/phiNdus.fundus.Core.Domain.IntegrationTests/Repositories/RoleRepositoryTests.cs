﻿using System;
using NHibernate.Exceptions;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    internal class RoleRepositoryTests : BaseTestFixture
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
                Assert.That(fromRepo.Name, Is.EqualTo("Benutzer"));
            }
        }

        [Test]
        public void Get_2_returns_Administrator_role()
        {
            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(2);
                Assert.That(fromRepo, Is.Not.Null);
                Assert.That(fromRepo.Name, Is.EqualTo("Administrator"));
            }
        }

        [Test]
        [ExpectedException(typeof(GenericADOException))]
        public void Update_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                fromRepo.Name = fromRepo.Name + " updated";
                Sut.Update(fromRepo);
                uow.TransactionalFlush();
            }
        }

        [Test]
        [ExpectedException(typeof(GenericADOException))]
        public void Save_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var role = new Role();
                role.Name = "New Role";
                Sut.Save(role);
                uow.TransactionalFlush();
            }
        }

        [Test]
        [ExpectedException(typeof(GenericADOException))]
        public void Delete_throws()
        {
            using (var uow = UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(1);
                fromRepo.Name = fromRepo.Name + " updated";
                Sut.Delete(fromRepo);
                uow.TransactionalFlush();
            }
        }
    }
}