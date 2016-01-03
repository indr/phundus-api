namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
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

        protected static IBorrowerService lesseeService;

        protected static LessorId lessorId = new LessorId();
        protected static Lessor lessor;

        protected static ILessorService lessorService;

        protected Establish dependencies = () =>
        {
            lessor = new Lessor(lessorId, "Lessor");
            memberInRole = depends.on<IMemberInRole>();
            orders = depends.on<IOrderRepository>();
            articles = depends.on<IArticleService>();
            lessorService = depends.on<ILessorService>();
            lesseeService = depends.on<IBorrowerService>();
        };
    }
}