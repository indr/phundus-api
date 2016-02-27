namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (RemoveOrderItemHandler))]
    public class when_remove_order_command_item_is_handled :
        order_command_handler_concern<RemoveOrderItem, RemoveOrderItemHandler>
    {
        private const int orderId = 2;
        private static OrderItemId orderItemId;
        private static Order order;

        public Establish c = () =>
        {
            var article = make.Article();
            order = new Order(theInitiator, new OrderId(), new OrderShortId(1234), theLessor, CreateLessee());
            orderItemId = new OrderItemId();
            order.AddItem(theInitiator, orderItemId, article, DateTime.Today, DateTime.Today, 1);
            orderRepository.setup(x => x.GetById(orderId)).Return(order);

            command = new RemoveOrderItem
            {
                InitiatorId = theInitiatorId,
                OrderId = orderId,
                OrderItemId = orderItemId.Id
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        public It should_publish_order_item_removed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderItemRemoved>.Is.NotNull));

        public It should_remove_order_item =
            () => order.Items.ShouldNotContain(p => p.Id == orderItemId.Id);
    }
}