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


        public ReservationSagaSteps(PastEvents pastEvents) : base(pastEvents)
        {
            PastEvents = pastEvents;
        }

        [Given(@"empty order created")]
        public void GivenEmptyOrderCreated()
        {
            PastEvents.Add(new OrderCreated(_orderId));
        }

        [When(@"order item added (.*?)")]
        public void WhenOrderItemAdded(Guid orderItemId)
        {
            _orderItemIds.Add(orderItemId);
            var evnt = new OrderItemAdded(_initiatorId, _organizationId, _orderId, orderItemId, _articleId, _period,
                _quantity);

            Transition(evnt);
        }

        [Then(@"reserve article")]
        public void ThenReserveArticle()
        {
            AssertUndispatchedCommand<ReserveArticle>(null);
        }

        [Given(@"order item added (.*?)")]
        public void GivenOrderItemAdded(Guid orderItemId)
        {
            PastEvents.Add(new OrderItemAdded(_initiatorId, _organizationId, _orderId, orderItemId, _articleId, _period,
                _quantity));

            _orderItemIds.Add(orderItemId);
        }

        [Then(@"no commands dispatched")]
        public void ThenNoCommandsDispatched()
        {
            AssertNoUndispatchedCommands();
        }

        [When(@"order item removed (.*?)")]
        public void WhenOrderItemRemoved(Guid orderItemId)
        {
            Transition(new OrderItemRemoved(_initiatorId, _organizationId, _orderId, orderItemId, _articleId));
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
            Transition(new OrderItemQuantityChanged(_initiatorId, _organizationId, _articleId, _orderId, orderItemId,
                _quantity, 2));
        }

        [Then(@"change reservation quantity (.*?)")]
        public void ThenChangeReservationQuantity(Guid reservationId)
        {
            AssertUndispatchedCommand<ChangeReservationQuantity>(null);
        }

        [When(@"reject order")]
        public void WhenRejectOrder()
        {
            Transition(new OrderRejected(_initiatorId, _organizationId, _orderId, _orderItemIds));
        }

        [When(@"close order")]
        public void WhenCloseOrder()
        {
            Transition(new OrderClosed(_initiatorId, _organizationId, _orderId, _orderItemIds));
        }
    }
}