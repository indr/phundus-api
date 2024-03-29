﻿namespace Phundus.Tests.Shop.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;

    [Subject(typeof (RemoveOrderItemHandler))]
    public class when_remove_order_command_item_is_handled :
        order_command_handler_concern<RemoveOrderItem, RemoveOrderItemHandler>
    {
        private const int orderId = 2;
        private static OrderLineId orderLineId;
        private static Order theOrder;

        public Establish c = () =>
        {
            var article = make.Product();
            theOrder = new Order(theManager, new OrderId(), new OrderShortId(1234), theLessor, theLessee);
            orderLineId = new OrderLineId();
            theOrder.AddItem(theManager, orderLineId, article, Period.FromNow(1), 1, 1);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            command = new RemoveOrderItem(theInitiatorId, theOrder.OrderId, orderLineId.Id);
        };

        public It should_remove_order_item =
            () => theOrder.Lines.ShouldNotContain(p => p.LineId.Id == orderLineId.Id);

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));
    }
}