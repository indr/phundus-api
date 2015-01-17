namespace Phundus.Core.Specs.Shop.Ordering
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Contexts;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ReservationSagaSteps : SagaConcern<ReservationSaga>
    {
        private readonly ArticleId _articleId = new ArticleId(1);
        private readonly UserId _initiatorId = new UserId(1);
        private readonly OrderId _orderId = new OrderId(1);
        private readonly IList<Guid> _orderItemIds = new List<Guid>();
        private readonly OrganizationId _organizationId = new OrganizationId(1);
        private readonly Period _period = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        private readonly int _quantity = 1;


        public ReservationSagaSteps(SagaContext context, PastEvents pastEvents) : base(context, pastEvents)
        {
            PastEvents = pastEvents;
        }

        [Given(@"empty order created")]
        public void GivenEmptyOrderCreated()
        {
            PastEvents.Add(new OrderCreated(_orderId));
        }
        
        [Given(@"order item added (.*?)")]
        public void GivenOrderItemAdded(Guid orderItemId)
        {
            PastEvents.Add(CreateOrderItemAdded(orderItemId));
        }

        [When(@"order item added (.*?)")]
        public void WhenOrderItemAdded(Guid orderItemId)
        {
            Transition(CreateOrderItemAdded(orderItemId));
        }

        private OrderItemAdded CreateOrderItemAdded(Guid orderItemId)
        {
            _orderItemIds.Add(orderItemId);
            return new OrderItemAdded(_initiatorId, _organizationId, _orderId, orderItemId, _articleId, _period,
                _quantity);
        }

        [Given(@"order item removed (.*?)")]
        public void GivenOrderItemRemoved(Guid orderItemId)
        {
            PastEvents.Add(CreateOrderItemRemoved(orderItemId));
        }

        [When(@"order item removed (.*?)")]
        public void WhenOrderItemRemoved(Guid orderItemId)
        {
            Transition(CreateOrderItemRemoved(orderItemId));
        }

        private OrderItemRemoved CreateOrderItemRemoved(Guid orderItemId)
        {
            return new OrderItemRemoved(_initiatorId, _organizationId, _orderId, orderItemId, _articleId);
        }

        [Then(@"reserve article")]
        public void ThenReserveArticle()
        {
            AssertUndispatchedCommand<ReserveArticle>(null);
        }

        [Then(@"cancel reservation (.*?)")]
        public void ThenCancelReservation(Guid reservationId)
        {
            var command = AssertUndispatchedCommand<CancelReservation>();
            Assert.That(command.ReservationId.Id, Is.EqualTo(reservationId.ToString()));
        }

        [When(@"order item period changed (.*?)")]
        public void WhenOrderItemPeriodChanged(Guid orderItemId)
        {
            Transition(new OrderItemPeriodChanged(_initiatorId, _organizationId, _orderId, orderItemId, _articleId,
                _period, new Period(DateTime.Today, DateTime.Today.AddDays(2))));
        }

        [Then(@"change reservation period (.*?)")]
        public void ThenChangeReservationPeriod(Guid reservationId)
        {
            var command = AssertUndispatchedCommand<ChangeReservationPeriod>();
            Assert.That(command.ReservationId.Id, Is.EqualTo(reservationId.ToString()));
        }

        [When(@"order item quantity changed (.*?)")]
        public void WhenOrderItemQuantityChanged(Guid orderItemId)
        {
            Transition(new OrderItemQuantityChanged(_initiatorId, _organizationId, _orderId, orderItemId, _articleId, 
                _quantity, _quantity + 1));
        }

        [Then(@"change reservation quantity (.*?)")]
        public void ThenChangeReservationQuantity(Guid reservationId)
        {
            AssertUndispatchedCommand<ChangeReservationQuantity>(null);
        }

        [Given(@"order rejected")]
        public void GivenRejectOrder()
        {
            PastEvents.Add(CreateOrderRejected());
        }
        
        [When(@"order rejected")]
        public void WhenRejectOrder()
        {
            Transition(CreateOrderRejected());
        }

        private OrderRejected CreateOrderRejected()
        {
            return new OrderRejected(_initiatorId, _organizationId, _orderId, _orderItemIds);
        }

        [Given(@"order closed")]
        public void GivenCloseOrder()
        {
            PastEvents.Add(CreateOrderClosed());
        }

        [When(@"order closed")]
        public void WhenCloseOrder()
        {
            Transition(CreateOrderClosed());
        }

        private OrderClosed CreateOrderClosed()
        {
            return new OrderClosed(_initiatorId, _organizationId, _orderId, _orderItemIds);
        }
    }
}