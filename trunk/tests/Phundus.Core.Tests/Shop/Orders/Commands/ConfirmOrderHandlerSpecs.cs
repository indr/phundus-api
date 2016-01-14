namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof(ApproveOrderHandler))]
    public class when_approve_order_is_handled : order_handler_concern<ApproveOrder, ApproveOrderHandler>
    {
        private const int initiatorId = 2;
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {            
            order = MockRepository.GeneratePartialMock<Order>(new object[] { lessor, BorrowerFactory.Create() });

            orders.setup(x => x.GetById(orderId)).Return(order);

            command = new ApproveOrder
            {
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(lessor.LessorId.Id, initiatorId));

        public It should_ask_order_to_approve =
            () => order.WasToldTo(x => x.Approve(initiatorId));

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Is.NotNull));
    }
}