namespace Phundus.Tests.Shop.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;

    [Subject(typeof (ApproveOrderHandler))]
    public class when_approve_order_command_is_handled :
        order_command_handler_concern<ApproveOrder, ApproveOrderHandler>
    {
        private static Order theOrder;

        public Establish c = () =>
        {
            theOrder = make.Order(theLessor);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            command = new ApproveOrder
            {
                InitiatorId = theInitiatorId,
                OrderId = theOrder.OrderId
            };
        };

        private OrderId orderId;

        public It should_approve_order =
            () => theOrder.received(x => x.Approve(theManager));

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));
    }
}