namespace Phundus.Tests.Inventory
{
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Inventory.Articles.Repositories;
    using Phundus.Inventory.Services;

    public abstract class article_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class,
            IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IArticleRepository articleRepository;

        protected static IOwnerService ownerService;

        protected static inventory_factory make;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);
            memberInRole = depends.on<IMemberInRole>();
            articleRepository = depends.on<IArticleRepository>();
            ownerService = depends.on<IOwnerService>();
        };
    }
}