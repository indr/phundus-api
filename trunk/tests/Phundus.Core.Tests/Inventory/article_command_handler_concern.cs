namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.Inventory.Model.Articles;
    using Phundus.Inventory.Model.Collaborators;

    public abstract class article_command_handler_concern<TCommand, THandler> :
        inventory_command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
        where TCommand : ICommand
    {
        protected static IMemberInRole memberInRole;

        protected static IArticleRepository articleRepository;

        protected static IOwnerService ownerService;

        

        private Establish ctx = () =>
        {
           
            memberInRole = depends.on<IMemberInRole>();
            articleRepository = depends.on<IArticleRepository>();
            ownerService = depends.on<IOwnerService>();
        };
    }
}