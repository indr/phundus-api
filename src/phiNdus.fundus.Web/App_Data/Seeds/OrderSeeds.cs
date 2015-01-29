namespace Phundus.Web.App_Data.Seeds
{
    using Core.Cqrs;
    using Core.Shop.Application.Commands;
    using Core.Shop.Domain.Model.Ordering;

    public class OrderSeeds : SeedStartupTask
    {
        public OrderSeeds(ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
        }

        protected override void Seed()
        {
            CreateEmptyOrder();
        }

        private OrderId CreateEmptyOrder()
        {
            var command = new CreateEmptyOrder(UserSeeds.Admin1, OrganizationSeeds.PfadiLego, UserSeeds.User1);
            Dispatch(command);
            return new OrderId(command.ResultingOrderId);
        }
    }
}