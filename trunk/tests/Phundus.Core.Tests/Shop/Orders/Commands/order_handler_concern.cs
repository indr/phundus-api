namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Common.Cqrs;
    using Core.Cqrs;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Identity;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using Machine.Specifications;

    public abstract class order_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository orders;

        protected static IArticleRepository articles;

        protected static IBorrowerService borrowerService;

        protected static OrganizationId organizationId = new OrganizationId(3);
        protected static Organization organization;

        protected Establish dependencies = () =>
        {
            organization = OrganizationFactory.Create(organizationId);
            memberInRole = depends.on<IMemberInRole>();
            orders = depends.on<IOrderRepository>();
            articles = depends.on<IArticleRepository>();
            borrowerService = depends.on<IBorrowerService>();
        };
    }
}