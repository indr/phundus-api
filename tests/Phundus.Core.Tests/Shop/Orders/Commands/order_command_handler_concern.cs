namespace Phundus.Tests.Shop.Orders.Commands
{
    using System;
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;
    using Phundus.Shop.Services;

    public abstract class order_command_handler_concern<TCommand, THandler> :
        shop_command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static IMemberInRole memberInRole;

        protected static IOrderRepository orderRepository;

        protected static IArticleService articleService;

        protected static ILesseeService lesseeService;

        protected static Lessor theLessor;

        protected static ILessorService lessorService;


        private Establish ctx = () =>
        {
            theLessor = make.Lessor();
            memberInRole = depends.on<IMemberInRole>();
            orderRepository = depends.on<IOrderRepository>();
            articleService = depends.on<IArticleService>();
            lessorService = depends.on<ILessorService>();
            lessorService.WhenToldTo(x => x.GetById(theLessor.LessorId)).Return(theLessor);
            lesseeService = depends.on<ILesseeService>();
        };

        protected static Lessee CreateLessee()
        {
            return CreateLessee(Guid.NewGuid());
        }

        protected static Lessee CreateLessee(Guid borrowerId)
        {
            return new Lessee(borrowerId, "Hans", "Muster", "Strasse", "6000", "Luzern", "hans.muster@test.phundus.ch",
                "+4179123456", "");
        }

        protected static Lessee CreateLessee(Guid borrowerId, string firstName, string lastName, string street = "",
            string postcode = "", string city = "", string emailAddress = "", string mobilePhoneNumber = "",
            string memberNumber = "")
        {
            return new Lessee(borrowerId, firstName, lastName, street, postcode, city, emailAddress, mobilePhoneNumber,
                memberNumber);
        }
    }
}