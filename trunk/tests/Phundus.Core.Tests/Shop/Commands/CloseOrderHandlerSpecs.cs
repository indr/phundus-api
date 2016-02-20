namespace Phundus.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Phundus.Tests.Shop;
    using Rhino.Mocks;

    [Subject(typeof (CloseOrderHandler))]
    public class when_close_order_command_is_handled : order_command_handler_concern<CloseOrder, CloseOrderHandler>
    {
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {
            order = make.Order(theLessor);
            orderRepository.setup(x => x.GetById(orderId)).Return(order);

            command = new CloseOrder
            {
                InitiatorId = theInitiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, theInitiatorId));

        public It should_ask_order_to_close =
            () => order.WasToldTo(x => x.Close(theInitiator));
    }
}