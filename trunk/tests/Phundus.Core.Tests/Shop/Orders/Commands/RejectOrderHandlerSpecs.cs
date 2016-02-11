namespace Phundus.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (RejectOrderHandler))]
    public class when_reject_order_command_is_handled : order_command_handler_concern<RejectOrder, RejectOrderHandler>
    {
        private static UserId initiatorId = new UserId();
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {
            order = make.Order(theLessor);
            orderRepository.setup(x => x.GetById(orderId)).Return(order);

            command = new RejectOrder
            {
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveManager(theLessor.LessorId.Id, initiatorId));

        public It should_ask_order_to_reject =
            () => order.WasToldTo(x => x.Reject(initiatorId));
    }
}