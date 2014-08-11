namespace Phundus.Core.Tests.Shop
{
    using Core.IdentityAndAccess.Users.Model;
    using Core.Shop.Contracts.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Orders.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;



    [Subject(typeof (CreateEmptyOrderHandler))]
    public class when_create_empty_order_is_handled :
        order_handler_concern<CreateEmptyOrder, CreateEmptyOrderHandler>
    {
        public const int organizationId = 1;
        public const int initiatorId = 2;
        public const int orderId = 3;
        public const int userId = 4;

        public Establish c = () =>
        {
            orders.setup(x => x.Add(Arg<Order>.Is.NotNull)).Return(orderId);
            borrowerService.setup(x => x.ById(userId)).Return(new Borrower(userId, "First", "Last", "mail@domain.tld"));
            userRepository.setup(x => x.ById(userId)).Return(new User(userId));
            command = new CreateEmptyOrder
            {
                OrganizationId = organizationId,
                InitiatorId = initiatorId,
                UserId = userId
            };
        };

        public It should_add_to_repository = () => orders.WasToldTo(x => x.Add(Arg<Order>.Is.NotNull));

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, initiatorId));

        public It should_publish_order_created = () => publisher.WasToldTo(x => x.Publish(
            Arg<OrderCreated>.Is.NotNull));

        public It should_set_order_id = () => command.OrderId.ShouldEqual(orderId);
    }
}