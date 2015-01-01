namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Application.Commands;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (RemoveOrderItemHandler))]
    public class when_remove_order_item_is_handled : order_handler_concern<RemoveOrderItem, RemoveOrderItemHandler>
    {
        private static UserId initiatorId = new UserId(1);
        private const int orderId = 2;
        private static Guid orderItemId;
        private static Order order;

        public Establish c = () =>
        {
            order = new Order(organization, BorrowerFactory.Create());
            orderItemId = order.AddItem(initiatorId, new Article(new ArticleId(1), organization.Id, "Artikel"), DateTime.Today, DateTime.Today.AddDays(1), 1).Id;
            orders.setup(x => x.GetById(orderId)).Return(order);

            command = new RemoveOrderItem
            {
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrderItemId = orderItemId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organization.Id, initiatorId.Id));

        public It should_publish_order_item_removed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemRemoved>.Is.NotNull));

        public It should_remove_order_item =
            () => order.Items.ShouldNotContain(p => p.Id == orderItemId);
    }
}