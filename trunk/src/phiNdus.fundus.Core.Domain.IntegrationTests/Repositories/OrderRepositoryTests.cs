using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class OrderRepositoryTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IOrderRepository>();
            Users = IoC.Resolve<IUserRepository>();
            Roles = IoC.Resolve<IRoleRepository>();
        }

        #endregion
        
        protected IOrderRepository Sut { get; set; }
        
        protected IUserRepository Users { get; set; }

        protected IRoleRepository Roles { get; set; }

        [Test]
        public void Can_save_and_load()
        {
            var orderId = 0;
            var order = new Order();

            using (var uow = UnitOfWork.Start())
            {
                order.Reserver = CreatePersistentUser("user@example.com");
                Sut.Save(order);
                orderId = order.Id;
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(orderId);

                Assert.That(fromRepo, Is.Not.Null);
                // Assert.That(fromRepo, Is.EqualTo(order))
                Assert.That(fromRepo.Id, Is.EqualTo(order.Id));
            }
        }

        [Test]
        public void Can_save_and_load_with_OrderItems()
        {
            
            var order = new Order();
            var item1 = CreateTransientOrderItem();
            var item2 = CreateTransientOrderItem();
            order.AddItem(item1);
            order.AddItem(item2);

            var orderId = 0;
            using (var uow = UnitOfWork.Start())
            {
                order.Reserver = CreatePersistentUser();
                item1.Article = CreatePersistentArticle();
                item2.Article = CreatePersistentArticle();
                Sut.Save(order);
                orderId = order.Id;
                uow.TransactionalFlush();
            }

            
            using (var uow = UnitOfWork.Start())
            {
                var fromRepo = Sut.Get(orderId);
                
                Assert.That(fromRepo, Is.Not.Null);
                // TODO: Assert.That(fromRepo, Is.EqualTo(order));
                Assert.That(fromRepo.Id, Is.EqualTo(orderId));
                Assert.That(fromRepo.Items, Has.Count.EqualTo(2));
            }

        }
    }
}