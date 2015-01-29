namespace Phundus.Web.App_Data.Seeds
{
    using Common.Domain.Model;
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
            var orderId = CreateEmptyOrder();
            AddOrderItem(orderId);
        }

        private OrderId CreateEmptyOrder()
        {
            var command = new CreateEmptyOrder(UserSeeds.Admin1, OrganizationSeeds.PfadiLego, UserSeeds.User1);
            Dispatch(command);
            return new OrderId(command.ResultingOrderId);
        }

        private OrderItemId AddOrderItem(OrderId orderId)
        {
            var command = new AddOrderItem(UserSeeds.Admin1, OrganizationSeeds.PfadiLego, orderId,
                ArticleSeeds.Slackline, Period.FromTodayToTomorrow, 1);
            Dispatch(command);
            return command.ResultingOrderItemId;
        }
    }
}