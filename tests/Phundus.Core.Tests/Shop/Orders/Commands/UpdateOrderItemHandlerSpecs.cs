namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using System.Linq;
    using Core.Inventory.Articles.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;
    using Article = Core.Shop.Orders.Model.Article;

    [Subject(typeof (UpdateOrderItemHandler))]
    public class when_update_order_item_is_handled : order_handler_concern<UpdateOrderItem, UpdateOrderItemHandler>
    {
        private const int initiatorId = 1;
        private const int orderId = 2;
        private const int newAmount = 20;
        private static Guid orderItemId;
        private static Order order;
        private static DateTime newFromUtc;
        private static DateTime newToUtc;

        public Establish c = () =>
        {
            var article = new Article(1, organization.Id, new Owner(new Guid(), "Owner"), "Artikel", 1.0m);
            order = new Order(lessor, BorrowerFactory.Create());
            orderItemId = order.AddItem(article, DateTime.Today, DateTime.Today, 1).Id;
            orders.setup(x => x.GetById(orderId)).Return(order);

            newFromUtc = DateTime.UtcNow.AddDays(1);
            newToUtc = DateTime.UtcNow.AddDays(2);
            command = new UpdateOrderItem
            {
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrderItemId = orderItemId,
                Amount = newAmount,
                FromUtc = newFromUtc,
                ToUtc = newToUtc
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organization.Id, initiatorId));

        public It should_publish_order_item_amount_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAmountChanged>.Is.NotNull));

        public It should_publish_order_item_period_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemPeriodChanged>.Is.NotNull));

        public It should_update_order_items_amount =
            () => order.Items.Single(p => p.Id == orderItemId).Amount.ShouldEqual(newAmount);

        public It should_update_order_items_period_from_to_midnight =
            () => order.Items.Single(p => p.Id == orderItemId).FromUtc.ShouldEqual(newFromUtc.ToLocalTime().Date.ToUniversalTime());

        public It should_update_order_items_period_to_one_second_before_midnight =
            () => order.Items.Single(p => p.Id == orderItemId).ToUtc.ShouldEqual(newToUtc.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime());
    }
}