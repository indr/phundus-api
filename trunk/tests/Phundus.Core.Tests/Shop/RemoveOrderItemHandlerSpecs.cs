namespace Phundus.Core.Tests.Shop
{
    using System;
    using Core.IdentityAndAccess.Users.Model;
    using Core.Inventory.Model;
    using Core.Shop.Contracts.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (RemoveOrderItemHandler))]
    public class when_remove_order_item_is_handled : order_handler_concern<RemoveOrderItem, RemoveOrderItemHandler>
    {
        private const int initiatorId = 1;
        private const int orderId = 2;
        private const int organizationId = 3;
        private static Guid orderItemId;
        private static Order order;

        public Establish c = () =>
        {
            order = new Order(organizationId, BorrowerFactory.Create());
            orderItemId = order.AddItem(new Article(organizationId, "Artikel"), DateTime.Today, DateTime.Today, 1).Id;
            orders.setup(x => x.ById(orderId)).Return(order);

            command = new RemoveOrderItem
            {
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrderItemId = orderItemId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_publish_order_item_removed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemRemoved>.Is.NotNull));

        public It should_remove_order_item =
            () => order.Items.ShouldNotContain(p => p.Id == orderItemId);
    }
}