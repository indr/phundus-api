using NUnit.Framework;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    public class OrderServiceTests : BaseTestFixture
    {
        #region SetUp/TearDown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FakeUnitOfWork = GenerateAndRegisterStubUnitOfWork();

            Sut = new OrderService();

            OrderDomainObject = new Order(1, 2);
        }

        #endregion

        protected OrderService Sut { get; set; }

        protected IUnitOfWork FakeUnitOfWork { get; set; }
        protected IOrderRepository FakeOrderRepo { get; set; }

        protected Order OrderDomainObject { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IOrderRepository>() == null)
            {
                FakeOrderRepo = GenerateAndRegisterStub<IOrderRepository>();
                FakeOrderRepo.Expect(x => x.Get(1)).Return(OrderDomainObject);
            }
        }

        [Test]
        public void Can_create()
        {
            var sut = new OrderService();
            Assert.That(sut, Is.Not.Null);
        }
        
        [Test]
        public void GetOrder_disposes_UnitOfWork()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();
            FakeUnitOfWork.Expect(x => x.Dispose());

            Sut.GetOrder(1);

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void GetOrder_calls_repository_Get()
        {
            FakeOrderRepo = GenerateAndRegisterMock<IOrderRepository>();
            GenerateAndRegisterMissingStubs();

            FakeOrderRepo.Expect(x => x.Get(1)).Return(null);
            Sut.GetOrder(1);

            FakeOrderRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetOrder_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            var actual = Sut.GetOrder(1);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.EqualTo(1));
            Assert.That(actual.Version, Is.EqualTo(2));
        }

        [Test]
        public void GetOrder_with_invalid_id_returns_null()
        {
            GenerateAndRegisterMissingStubs();

            Assert.That(Sut.GetOrder(404), Is.Null);
        }

        
    }
}