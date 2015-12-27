namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using System;
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using Core.Shop.Services;
    using Machine.Specifications;

    public abstract class order_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository orders;

        protected static IArticleService articles;

        protected static IBorrowerService borrowerService;

        [Obsolete]
        protected static Organization organization;

        protected static Lessor lessor;

        protected Establish dependencies = () =>
        {
            lessor = new Lessor(Guid.NewGuid(), "Lessor");
            organization = OrganizationFactory.Create();
            memberInRole = depends.on<IMemberInRole>();
            orders = depends.on<IOrderRepository>();
            articles = depends.on<IArticleService>();
            borrowerService = depends.on<IBorrowerService>();
        };
    }
}