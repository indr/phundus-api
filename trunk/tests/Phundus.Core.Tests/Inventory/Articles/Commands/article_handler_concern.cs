namespace Phundus.Core.Tests.Inventory
{
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Inventory.Articles.Repositories;
    using Phundus.Inventory.Services;

    public abstract class article_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IArticleRepository repository;

        protected static IOwnerService ownerService;

        protected Establish dependencies = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IArticleRepository>();
            ownerService = depends.on<IOwnerService>();
        };
    }
}