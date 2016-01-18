namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Shop.Orders.Commands;
    using Phundus.Shop.Orders.Model;
    using Rhino.Mocks;

    [Subject(typeof (CreateEmptyOrderHandler))]
    public class when_create_empty_order_is_handled :
        order_handler_concern<CreateEmptyOrder, CreateEmptyOrderHandler>
    {
        public const int orderId = 3;
        public static Guid userId = Guid.NewGuid();
        public static CurrentUserId initiatorId = new CurrentUserId();

        public Establish c = () =>
        {
            var lessee = CreateLessee(userId);
            orderRepository.setup(x => x.Add(Arg<Order>.Is.NotNull)).Return(orderId);
            lessorService.setup(x => x.GetById(theLessor.LessorId)).Return(theLessor);
            lesseeService.setup(x => x.GetById(lessee.LesseeId)).Return(lessee);

            command = new CreateEmptyOrder
            {
                LessorId = theLessor.LessorId,
                InitiatorId = initiatorId,
                LesseeId = new LesseeId(userId)
            };
        };

        public It should_add_to_repository = () => orderRepository.WasToldTo(x => x.Add(Arg<Order>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(new OwnerId(theLessor.LessorId.Id), initiatorId));

        public It should_publish_order_created = () => publisher.WasToldTo(x => x.Publish(
            Arg<OrderCreated>.Matches(p => p.OrderId == orderId)));

        public It should_set_order_id = () => command.ResultingOrderId.ShouldEqual(orderId);
    }
}