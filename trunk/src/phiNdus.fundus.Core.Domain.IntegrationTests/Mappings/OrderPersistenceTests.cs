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
        [Test]
        public void Can_save_and_load()
        {
            var sut = new Order();
            Order fromSession = null;
            int id = 0;

            using (var uow = UnitOfWork.Start())
            {
                sut.Reserver = CreateOrGetUser("user@example.com");
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
