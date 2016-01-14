namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Contracts.Repositories;
    using Phundus.Shop.Services;

    public abstract class contract_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IContractRepository repository;

        protected static ILesseeService lesseeService;

        protected Establish dependencies = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IContractRepository>();
            lesseeService = depends.on<ILesseeService>();
        };
    }
}