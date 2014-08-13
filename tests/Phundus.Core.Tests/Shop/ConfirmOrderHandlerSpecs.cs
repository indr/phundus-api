namespace Phundus.Core.Tests.Shop
{
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof(ConfirmOrderHandler))]
    public class when_confirm_order_is_handled : order_handler_concern<ConfirmOrder, ConfirmOrderHandler>
    {
        private const int organizationId = 1;
        private const int initiatorId = 2;
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {
            order = MockRepository.GeneratePartialMock<Order>(new object[] { organizationId, BorrowerFactory.Create() });
            orders.setup(x => x.GetById(orderId)).Return(order);

            command = new ConfirmOrder
            {
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_ask_order_to_confirm =
            () => order.WasToldTo(x => x.Confirm());

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Is.NotNull));
    }
}