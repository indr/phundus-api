namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Common.Cqrs;
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Domain.Model.Identity;
    using Core.Shop.Domain.Model.Renting;
    using Machine.Specifications;

    public abstract class contract_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
    {
        protected static IMemberInRole memberInRole;

        protected static IContractRepository repository;

        protected static IBorrowerService borrowerService;

        protected Establish dependencies = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IContractRepository>();
            borrowerService = depends.on<IBorrowerService>();
        };
    }
}