namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Identity;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using Machine.Specifications;

    public abstract class order_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository orders;

        protected static IArticleRepository articles;

        protected static IBorrowerService borrowerService;

        protected static Organization organization;

        protected Establish dependencies = () =>
        {
            organization = OrganizationFactory.Create();
            memberInRole = depends.on<IMemberInRole>();
            orders = depends.on<IOrderRepository>();
            articles = depends.on<IArticleRepository>();
            borrowerService = depends.on<IBorrowerService>();
        };
    }
}