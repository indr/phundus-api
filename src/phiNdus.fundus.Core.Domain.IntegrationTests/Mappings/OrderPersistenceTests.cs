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
                sut.Reserver = CreateAndPersistUser("user@example.com");
                UnitOfWork.CurrentSession.Save(sut);
                id = sut.Id;
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                fromSession = UnitOfWork.CurrentSession.Get<Order>(id);
            }

            // TODO: Assert.That(fromSession, Is.EqualTo(sut));
            Assert.That(fromSession, Is.Not.Null);
            Assert.That(fromSession.Id, Is.EqualTo(sut.Id));
        }

        [Test]
        public void Can_save_and_load_with_OrderItems()
        {
            var sut = new Order();
            var item1 = CreateTransientOrderItem();
            item1.Amount = 1;
            var item2 = CreateTransientOrderItem();
            item2.Amount = 1;
            Assert.True(sut.AddItem(item1));
            Assert.True(sut.AddItem(item2));


            var orderId = 0;
            using (var uow = UnitOfWork.Start())
            {
                sut.Reserver = CreateAndPersistUser();
                item1.Article = CreatePersistentArticle();
                item2.Article = CreatePersistentArticle();
                UnitOfWork.CurrentSession.Save(sut);
                orderId = sut.Id;
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                var fromSession = UnitOfWork.CurrentSession.Get<Order>(orderId);
                Assert.That(fromSession, Is.Not.Null);
                Assert.That(fromSession.Items, Has.Count.EqualTo(2));
            }

            
        }
    }
}
