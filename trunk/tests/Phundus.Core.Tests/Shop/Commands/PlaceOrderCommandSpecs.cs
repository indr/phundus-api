namespace Phundus.Tests.Shop.Commands
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Orders.Commands;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    public class when_placing_order_is_handled : order_handler_concern<PlaceOrder, PlaceOrderHandler>
    {
        private static UserId theInitiatorId;
        private static LessorId theLessorId;
        private static Lessor theLessor;

        private Establish ctx = () =>
        {
            theInitiatorId = new UserId(1);

            theLessorId = new LessorId();
            theLessor = new Lessor(theLessorId, "Lessor");
            lessorService.WhenToldTo(x => x.GetById(theLessorId)).Return(theLessor);

            lesseeService.WhenToldTo(x => x.GetById(new LesseeId(theInitiatorId.Id)))
                .Return(CreateLessee(theInitiatorId.Id));

            command = new PlaceOrder(theInitiatorId, theLessorId);
        };

        private It should_add_to_repository = () => orderRepository.WasToldTo(x => x.Add(Arg<Order>.Is.NotNull));

        [Ignore("wip")]
        private It should_publish_order_placed = () => publisher.WasToldTo(x => x.Publish(Arg<OrderPlaced>.Is.NotNull));
    }
}