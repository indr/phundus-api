namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Tests.Shop;
    using Rhino.Mocks;
    using Article = Phundus.Shop.Orders.Model.Article;

    [Subject(typeof (RemoveOrderItemHandler))]
    public class when_remove_order_command_item_is_handled : order_command_handler_concern<RemoveOrderItem, RemoveOrderItemHandler>
    {
        private static UserId initiatorId = new UserId();
        private const int orderId = 2;
        private static OrderItemId orderItemId;
        private static Order order;

        public Establish c = () =>
        {
            var article = new Article(1, new Owner(new OwnerId(Guid.NewGuid()), "Owner"), "Artikel", 1.0m);
            order = new Order(theLessor, CreateLessee());
            orderItemId = new OrderItemId();
            order.AddItem(orderItemId, article, DateTime.Today, DateTime.Today, 1);
            orderRepository.setup(x => x.GetById(orderId)).Return(order);

            command = new RemoveOrderItem
            {
                InitiatorId = initiatorId,
                OrderId = orderId,
                OrderItemId = orderItemId.Id
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(theLessor.LessorId.Id, initiatorId));

        public It should_publish_order_item_removed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemRemoved>.Is.NotNull));

        public It should_remove_order_item =
            () => order.Items.ShouldNotContain(p => p.Id == orderItemId.Id);
    }
}