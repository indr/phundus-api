namespace Phundus.Core.Tests.Shop.Orders.Commands
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;
    using Phundus.Shop.Services;

    public abstract class order_handler_concern<TCommand, THandler> : handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository orders;

        protected static IArticleService articles;

        protected static ILesseeService lesseeService;

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
            lesseeService = depends.on<ILesseeService>();
        };
    }
}