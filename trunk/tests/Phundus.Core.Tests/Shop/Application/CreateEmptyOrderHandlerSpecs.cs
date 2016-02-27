namespace Phundus.Tests.Shop.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
    using Rhino.Mocks;

    [Subject(typeof (CreateEmptyOrderHandler))]
    public class when_create_empty_order_command_is_handled :
        order_command_handler_concern<CreateEmptyOrder, CreateEmptyOrderHandler>
    {
        private static OrderId theOrderId;
        private static OrderShortId theOrderShortId;

        public Establish c = () =>
        {
            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);

            command = new CreateEmptyOrder(theInitiatorId, theOrderId, theOrderShortId,
                theLessor.LessorId, theLessee.LesseeId);
        };

        public It should_add_to_repository = () =>
            orderRepository.received(x => x.Add(Arg<Order>.Is.NotNull));
    }
}