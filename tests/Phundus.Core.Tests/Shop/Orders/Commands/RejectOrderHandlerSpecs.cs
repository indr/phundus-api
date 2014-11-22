namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Core.Shop.Application.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (RejectOrderHandler))]
    public class when_reject_order_is_handled : order_handler_concern<RejectOrder, RejectOrderHandler>
    {
        private const int initiatorId = 2;
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {
            order = mock.partial<Order>(new object[] {organization, BorrowerFactory.Create()});
            orders.setup(x => x.GetById(orderId)).Return(order);

            command = new RejectOrder
            {
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organization.Id, initiatorId));

        public It should_ask_order_to_reject =
            () => order.WasToldTo(x => x.Reject(initiatorId));

        public It should_publish_order_rejected =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderRejected>.Is.NotNull));
    }
}