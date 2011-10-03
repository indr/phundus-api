using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    public class OrderPersistenceTests : BaseTestFixture
    {

        public User CreatePersistetUser()
        {
            var result = new User();
            result.Membership.Email = "user@example.com";
            using (var uow = UnitOfWork.Start())
            {
                var session = UnitOfWork.CurrentSession;
                result.Role = session.Query<Role>().First();
                session.Save(result.Membership);
                session.Save(result);
            }
            return result;
        }


        [Test]
        public void Can_save_and_load()
        {
            var sut = new Order();
            Order fromSession = null;
            int id = 0;

            using (var uow = UnitOfWork.Start())
            {
                sut.Reserver = CreatePersistetUser();
                UnitOfWork.CurrentSession.Save(sut);
                id = sut.Id;
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                fromSession = UnitOfWork.CurrentSession.Get<Order>(id);
            }

            // TODO: Assert.That(fromSession, Is.EqualTo(sut));
            Assert.That(fromSession.Id, Is.EqualTo(sut.Id));
        }
    }
}
