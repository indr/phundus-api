namespace Phundus.Tests.Shop.Application
{
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;

    [Subject(typeof (RejectOrderHandler))]
    public class when_reject_order_command_is_handled : order_command_handler_concern<RejectOrder, RejectOrderHandler>
    {
        private static Order theOrder;

        public Establish c = () =>
        {
            theOrder = make.Order(theLessor);
            orderRepository.setup(x => x.GetById(theOrder.OrderId)).Return(theOrder);

            command = new RejectOrder
            {
                InitiatorId = theInitiatorId,
                OrderId = theOrder.OrderId
            };
        };

        public It should_reject_order =
            () => theOrder.received(x => x.Reject(theManager));

        private It should_save_to_repository = () =>
            orderRepository.received(x => x.Save(theOrder));
    }
}