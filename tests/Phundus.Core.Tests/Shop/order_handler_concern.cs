namespace Phundus.Core.Tests.Shop
{
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.IdentityAndAccess.Users.Repositories;
    using Core.Inventory.Repositories;
    using Core.Shop.Contracts.Services;
    using Core.Shop.Orders.Repositories;
    using Machine.Specifications;

    public abstract class order_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository orders;

        protected static IArticleRepository articles;

        protected static IBorrowerService borrowerService;

        protected Establish dependencies = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            orders = depends.on<IOrderRepository>();
            articles = depends.on<IArticleRepository>();
            borrowerService = depends.on<IBorrowerService>();
        };
    }
}