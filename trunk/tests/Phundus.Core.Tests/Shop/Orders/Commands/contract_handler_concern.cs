namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Core.Cqrs;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Contracts.Repositories;
    using Core.Shop.Domain.Model.Identity;
    using Machine.Specifications;

    public abstract class contract_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
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