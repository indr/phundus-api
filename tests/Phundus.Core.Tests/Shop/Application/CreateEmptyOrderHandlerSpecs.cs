namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Application;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (CreateEmptyOrderHandler))]
    public class when_create_empty_order_command_is_handled :
        order_command_handler_concern<CreateEmptyOrder, CreateEmptyOrderHandler>
    {
        public static Guid userId = Guid.NewGuid();
        private static OrderId theOrderId;
        private static OrderShortId theOrderShortId;

        public Establish c = () =>
        {
            var lessee = CreateLessee(new LesseeId(userId));

            theOrderId = new OrderId();
            theOrderShortId = new OrderShortId(1234);

            lessorService.setup(x => x.GetById(theLessor.LessorId)).Return(theLessor);
            lesseeService.setup(x => x.GetById(lessee.LesseeId)).Return(lessee);

            command = new CreateEmptyOrder(theInitiatorId, theOrderId, theOrderShortId,
                theLessor.LessorId, new LesseeId(userId));
        };

        public It should_add_to_repository = () =>
            orderRepository.received(x => x.Add(Arg<Order>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveManager(new OwnerId(theLessor.LessorId.Id), theInitiatorId));
    }
}