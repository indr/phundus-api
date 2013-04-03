using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.Builders;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.ServicesTests.OrderServiceTests
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;

    [TestFixture]
    public class GetAndFindTests : UnitTestBase<Business.Services.OrderService>
    {
        #region SetUp/TearDown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FakeUnitOfWork = GenerateAndRegisterStubUnitOfWork();

            Sut = new Business.Services.OrderService();
            var user = new User();
            Sut.SecurityContext = new SecurityContextBuilder().ForUser(user).Build();
            user.SelectedOrganization = new Organization(1);

            OrderDomainObject1 = new Order(1, 2);
            OrderDomainObject2 = new Order(2, 3);
        }

        #endregion

        protected IUnitOfWork FakeUnitOfWork { get; set; }
        protected IOrderRepository FakeOrderRepo { get; set; }

        protected Order OrderDomainObject1 { get; set; }
        protected Order OrderDomainObject2 { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (GlobalContainer.TryResolve<IOrderRepository>() == null)
            {
                FakeOrderRepo = GenerateAndRegisterStub<IOrderRepository>();
                FakeOrderRepo.Expect(x => x.Get(1)).Return(OrderDomainObject1);
                FakeOrderRepo.Expect(x => x.FindPending(Arg<Organization>.Is.Anything)).Return(new System.Collections.Generic.List<Order>
                                                                     {
                                                                         OrderDomainObject1,
                                                                         OrderDomainObject2
                                                                     });
                FakeOrderRepo.Expect(x => x.FindApproved(Arg<Organization>.Is.Anything)).Return(new System.Collections.Generic.List<Order>
                                                                     {
                                                                         OrderDomainObject1,
                                                                         OrderDomainObject2
                                                                     });
                FakeOrderRepo.Expect(x => x.FindRejected(Arg<Organization>.Is.Anything)).Return(new System.Collections.Generic.List<Order>
                                                                     {
                                                                         OrderDomainObject1,
                                                                         OrderDomainObject2
                                                                     });
            }
        }

        [Test]
        public void Can_create()
        {
            var sut = new Business.Services.OrderService();
            Assert.That(sut, Is.Not.Null);
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
        public void GetOrder_disposes_UnitOfWork()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();
            FakeUnitOfWork.Expect(x => x.Dispose());

            Sut.GetOrder(1);

            FakeUnitOfWork.VerifyAllExpectations();
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

        [Test]
        public void GetPending_calls_repository_FindPendings()
        {
            FakeOrderRepo = GenerateAndRegisterMock<IOrderRepository>();
            GenerateAndRegisterMissingStubs();

            FakeOrderRepo.Expect(x => x.FindPending(Arg<Organization>.Is.Anything)).Return(new System.Collections.Generic.List<Order>());
            Sut.GetPending();

            FakeOrderRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetPending_disposes_UnitOfWork()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();
            FakeUnitOfWork.Expect(x => x.Dispose());

            Sut.GetPending();

            FakeUnitOfWork.VerifyAllExpectations();            
        }

        [Test]
        public void GetPending_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var actual = Sut.GetPending();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetApproved_calls_repository_FindApproved()
        {
            FakeOrderRepo = GenerateAndRegisterMock<IOrderRepository>();
            GenerateAndRegisterMissingStubs();

            FakeOrderRepo.Expect(x => x.FindApproved(Arg<Organization>.Is.Anything)).Return(new System.Collections.Generic.List<Order>());
            Sut.GetApproved();

            FakeOrderRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetApproved_disposes_UnitOfWork()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();
            FakeUnitOfWork.Expect(x => x.Dispose());

            Sut.GetApproved();

            FakeUnitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void GetApproved_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var actual = Sut.GetApproved();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetRejected_calls_repository_FindRejected()
        {
            FakeOrderRepo = GenerateAndRegisterMock<IOrderRepository>();
            GenerateAndRegisterMissingStubs();

            FakeOrderRepo.Expect(x => x.FindRejected(Arg<Organization>.Is.Anything)).Return(new System.Collections.Generic.List<Order>());
            Sut.GetRejected();

            FakeOrderRepo.VerifyAllExpectations();
        }

        [Test]
        public void GetRejected_disposes_UnitOfWork()
        {
            FakeUnitOfWork = GenerateAndRegisterMockUnitOfWork();
            GenerateAndRegisterMissingStubs();
            FakeUnitOfWork.Expect(x => x.Dispose());

            Sut.GetRejected();

            FakeUnitOfWork.VerifyAllExpectations();            
        }

        [Test]
        public void GetRejected_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            var actual = Sut.GetRejected();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Has.Count.EqualTo(2));
        }
    }
}