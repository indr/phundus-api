using System;
using System.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.SecuredServices
{
    [TestFixture]
    public class SecuredOrderServiceTests : BaseTestFixture
    {
        #region SetUp/TearDown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            GenerateAndRegisterStubUnitOfWork();

            Sut = new SecuredOrderService();
        }

        #endregion

        private IOrderService Sut { get; set; }

        private OrderService FakeOrderService { get; set; }
        private IUserRepository FakeUserRepo { get; set; }

        protected User SessionAdmin { get; set; }
        protected User SessionUser { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<OrderService>() == null)
                FakeOrderService = GenerateAndRegisterStub<OrderService>();
            if (IoC.TryResolve<IUserRepository>() == null)
                FakeUserRepo = GenerateAndRegisterStub<IUserRepository>();

            if (SessionAdmin == null)
            {
                SessionAdmin = new User();
                SessionAdmin.Role = Role.Administrator;
            }
            if (SessionUser == null)
            {
                SessionUser = new User();
                SessionUser.Role = Role.User;
            }
        }

        [Test]
        public void GetOrder_calls_GetOrder()
        {
            FakeOrderService = GenerateAndRegisterStrictMock<OrderService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetOrder(1)).Return(null);
            Sut.GetOrder("valid", 1);

            FakeOrderService.VerifyAllExpectations();
        }

        [Test]
        public void GetOrder_others_with_user_roll_throws()
        {
            Assert.Ignore("To be implemented");
        }

        [Test]
        public void GetOrder_own_with_user_roll_does_not_throw()
        {
            Assert.Ignore("To be implemented");
        }

        [Test]
        public void GetOrder_returns_dto()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetOrder(1)).Return(new OrderDto());
            var actual = Sut.GetOrder("valid", 1);

            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void GetOrder_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetOrder("invalid", 0));
        }

        [Test]
        public void GetOrder_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetOrder(null, 0));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetPendingOrders_calls_GetPending()
        {
            FakeOrderService = GenerateAndRegisterStrictMock<OrderService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetPending()).Return(null);
            Sut.GetPendingOrders("valid");

            FakeOrderService.VerifyAllExpectations();
        }

        [Test]
        public void GetPendingOrders_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetPending()).Return(new OrderDto[2]);
            IList<OrderDto> actual = Sut.GetPendingOrders("valid");

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetPendingOrders_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetPendingOrders("invalid"));
        }

        [Test]
        public void GetPendingOrders_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetPendingOrders(null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetPendingOrders_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionUser);
            Assert.Throws<AuthorizationException>(() => Sut.GetPendingOrders("valid"));
        }

        [Test]
        public void GetApprovedOrders_calls_GetApproved()
        {
            FakeOrderService = GenerateAndRegisterStrictMock<OrderService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetApproved()).Return(null);
            Sut.GetApprovedOrders("valid");

            FakeOrderService.VerifyAllExpectations();
        }

        [Test]
        public void GetApprovedOrders_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetApproved()).Return(new OrderDto[2]);
            IList<OrderDto> actual = Sut.GetApprovedOrders("valid");
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetApprovedOrders_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetApprovedOrders("invalid"));
        }

        [Test]
        public void GetApprovedOrders_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetApprovedOrders(null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetApprovedOrders_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionUser);
            Assert.Throws<AuthorizationException>(() => Sut.GetApprovedOrders("valid"));
        }

        [Test]
        public void GetRejectedOrders_calls_GetPending()
        {
            FakeOrderService = GenerateAndRegisterStrictMock<OrderService>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetRejected()).Return(null);
            Sut.GetRejectedOrders("valid");

            FakeOrderService.VerifyAllExpectations();
        }

        [Test]
        public void GetRejectedOrders_returns_dtos()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionAdmin);
            FakeOrderService.Expect(x => x.GetRejected()).Return(new OrderDto[2]);
            IList<OrderDto> actual = Sut.GetRejectedOrders("valid");

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count, Is.EqualTo(2));

        }

        [Test]
        public void GetRejectedOrders_with_invalid_sessionKey_throws()
        {
            GenerateAndRegisterMissingStubs();

            Assert.Throws<InvalidSessionKeyException>(() => Sut.GetRejectedOrders("invalid"));
        }

        [Test]
        public void GetRejectedOrders_with_sessionKey_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetRejectedOrders(null));
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetRejectedOrders_with_user_roll_throws()
        {
            GenerateAndRegisterMissingStubs();

            FakeUserRepo.Expect(x => x.FindBySessionKey("valid")).Return(SessionUser);
            Assert.Throws<AuthorizationException>(() => Sut.GetRejectedOrders("valid"));
        }

        
    }
}