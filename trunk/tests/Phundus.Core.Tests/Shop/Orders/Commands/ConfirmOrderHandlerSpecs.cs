namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof(ApproveOrderHandler))]
    public class when_approve_order_is_handled : order_handler_concern<ApproveOrder, ApproveOrderHandler>
    {
        private const int initiatorId = 2;
        private const int orderId = 3;
        private static Order order;

        public Establish c = () =>
        {
            organization = OrganizationFactory.Create();
            order = MockRepository.GeneratePartialMock<Order>(new object[] { organization, BorrowerFactory.Create() });

            orders.setup(x => x.GetById(orderId)).Return(order);

            command = new ApproveOrder
            {
                InitiatorId = initiatorId,
                OrderId = orderId
            };
        };

        public It should_ask_for_chief_privilegs =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organization.Id, initiatorId));

        public It should_ask_order_to_approve =
            () => order.WasToldTo(x => x.Approve(initiatorId));

        public It should_publish_order_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrderApproved>.Is.NotNull));
    }
}