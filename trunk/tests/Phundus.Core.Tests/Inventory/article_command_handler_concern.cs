namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Projections;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Articles;

    public abstract class article_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
        where TCommand : ICommand
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