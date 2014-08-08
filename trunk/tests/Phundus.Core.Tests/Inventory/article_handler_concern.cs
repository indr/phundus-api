namespace Phundus.Core.Tests.Inventory
{
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Repositories;
    using Machine.Specifications;

    public abstract class article_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IArticleRepository repository;

        protected Establish dependencies = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IArticleRepository>();
        };
    }
}