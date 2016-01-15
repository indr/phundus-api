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
    public class when_close_order_is_handled : order_handler_concern<CloseOrder, CloseOrderHandler>
    {
        private static UserGuid initiatorId = new UserGuid();
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {
            order = MockRepository.GeneratePartialMock<Order>(new object[] {theLessor, CreateLessee()});
            orderRepository.setup(x => x.GetById(orderId)).Return(order);

            command = new CloseOrder
            {
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(theLessor.LessorId.Id, initiatorId));

        public It should_ask_order_to_close =
            () => order.WasToldTo(x => x.Close(initiatorId));

        public It should_publish_order_closed =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderClosed>.Is.NotNull));
    }
}