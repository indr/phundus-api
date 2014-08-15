namespace Phundus.Core.Tests.Shop
{
    using System;
    using System.Linq;
    using Core.IdentityAndAccess.Users.Model;
    using Core.Inventory.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (UpdateOrderItemHandler))]
    public class when_update_order_item_is_handled : order_handler_concern<UpdateOrderItem, UpdateOrderItemHandler>
    {
        private const int initiatorId = 1;
        private const int orderId = 2;
        private const int organizationId = 3;
        private const int newAmount = 20;
        private static Guid orderItemId;
        private static Order order;
        private static DateTime newFrom;
        private static DateTime newTo;

        public Establish c = () =>
        {
            order = new Order(OrganizationFactory.Create(), BorrowerFactory.Create());
            orderItemId = order.AddItem(new Article(organizationId, "Artikel"), DateTime.Today, DateTime.Today, 1).Id;
            orders.setup(x => x.GetById(orderId)).Return(order);

            newFrom = DateTime.UtcNow.AddDays(1);
            newTo = DateTime.UtcNow.AddDays(2);
            command = new UpdateOrderItem
            {
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrderItemId = orderItemId,
                Amount = newAmount,
                From = newFrom,
                To = newTo
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_publish_order_item_amount_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemAmountChanged>.Is.NotNull));

        public It should_publish_order_item_period_changed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemPeriodChanged>.Is.NotNull));

        public It should_update_order_items_amount =
            () => order.Items.Single(p => p.Id == orderItemId).Amount.ShouldEqual(newAmount);

        public It should_update_order_items_period_from =
            () => order.Items.Single(p => p.Id == orderItemId).From.ShouldEqual(newFrom);

        public It should_update_order_items_period_to =
            () => order.Items.Single(p => p.Id == orderItemId).To.ShouldEqual(newTo);
    }
}