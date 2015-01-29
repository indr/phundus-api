namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Application.Commands;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (UpdateOrderItemHandler))]
    public class when_update_order_item_is_handled : order_handler_concern<UpdateOrderItem, UpdateOrderItemHandler>
    {
        private static UserId initiatorId = new UserId(1);
        private static OrderId orderId = new OrderId(2);
        private const int newAmount = 20;
        private static OrderItemId orderItemId;
        private static Order order;
        private static DateTime newFromUtc;
        private static DateTime newToUtc;

        public Establish c = () =>
        {
            order = new Order(organization, BorrowerFactory.Create());
            orderItemId = new OrderItemId(order.AddItem(initiatorId, new Article(new ArticleId(1),  organization.Id, "Artikel"), DateTime.Today, DateTime.Today.AddDays(1), 1).Id);
            orders.setup(x => x.GetById(orderId.Id)).Return(order);

            newFromUtc = DateTime.UtcNow.AddDays(1);
            newToUtc = DateTime.UtcNow.AddDays(2);
            command = new UpdateOrderItem(initiatorId, organizationId, orderId, orderItemId,
                new Period(newFromUtc, newToUtc), newAmount);
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organization.Id, initiatorId.Id));

        public It should_publish_order_item_amount_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemQuantityChanged>.Is.NotNull));

        public It should_publish_order_item_period_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemPeriodChanged>.Is.NotNull));

        public It should_update_order_items_amount =
            () => order.Items.Single(p => p.Id == orderItemId.Id).Amount.ShouldEqual(newAmount);

        public It should_update_order_items_period_from_to_midnight =
            () => order.Items.Single(p => p.Id == orderItemId.Id).FromUtc.ShouldEqual(newFromUtc.ToLocalTime().Date.ToUniversalTime());

        public It should_update_order_items_period_to_one_second_before_midnight =
            () => order.Items.Single(p => p.Id == orderItemId.Id).ToUtc.ShouldEqual(newToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());
    }
}