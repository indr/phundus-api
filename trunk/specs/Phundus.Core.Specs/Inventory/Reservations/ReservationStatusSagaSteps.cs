namespace Phundus.Core.Specs.Inventory.Reservations
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReservationStatusSagaSteps : SagaConcern<ReservationStatusSaga>
    {
        private UserId _initiatorId = new UserId(101);
        private OrganizationId _organizationId = new OrganizationId(1001);
        private OrderId _orderId = new OrderId(100001);
        private Guid _orderItemId = Guid.NewGuid();
        private ArticleId _articleId = new ArticleId(10001);
        private StockId _stockId = new StockId("Stock-1234");
        private AllocationId _allocationId = new AllocationId();
        private ReservationId _reservationId;
        private Period _period = Period.FromTodayToTomorrow;
        private int _quantity = 1;

        protected ReservationStatusSagaSteps(SagaContext context, PastEvents pastEvents) : base(context, pastEvents)
        {
            _reservationId = new ReservationId(_allocationId.Id);
        }

        [When(@"stock allocated")]
        public void WhenStockAllocated()
        {
            Transition(CreateStockAllocated());
        }

        private StockAllocated CreateStockAllocated()
        {
            return new StockAllocated(_organizationId, _articleId, _stockId, _allocationId, _reservationId, _period, _quantity, AllocationStatus.Unknown);
        }

        [Given(@"order approved")]
        public void GivenOrderApproved()
        {
            PastEvents.Add(CreateOrderApproved());
        }

        [When(@"order approved")]
        public void WhenOrderApproved()
        {
            Transition(CreateOrderApproved());
        }

        private OrderApproved CreateOrderApproved()
        {
            return new OrderApproved(_initiatorId, _organizationId, _orderId, new List<Guid> {_orderItemId});
        }
    }
}