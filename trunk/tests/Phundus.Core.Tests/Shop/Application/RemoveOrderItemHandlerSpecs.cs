namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (RemoveOrderItemHandler))]
    public class when_remove_order_command_item_is_handled :
        order_command_handler_concern<RemoveOrderItem, RemoveOrderItemHandler>
    {
        private const int orderId = 2;
        private static OrderLineId orderLineId;
        private static Order theOrder;

        public Establish c = () =>
        {
            var article = make.Article();
            theOrder = new Order(theInitiator, new OrderId(), new OrderShortId(1234), theLessor, CreateLessee());
            orderLineId = new OrderLineId();
            theOrder.AddItem(theInitiator, orderLineId, article, Period.FromNow(1), 1);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            command = new RemoveOrderItem
            {
                InitiatorId = theInitiatorId,
                OrderId = theOrder.OrderId,
                OrderItemId = orderLineId.Id
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        public It should_remove_order_item =
            () => theOrder.Lines.ShouldNotContain(p => p.LineId.Id == orderLineId.Id);

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));
    }
}