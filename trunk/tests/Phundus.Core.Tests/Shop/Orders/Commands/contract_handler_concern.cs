namespace Phundus.Tests.Shop.Orders.Commands
{
    using Machine.Specifications;
    using Phundus.Cqrs;
    using Phundus.IdentityAccess.Queries;
    using Phundus.Shop.Contracts.Model;
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

        protected static Lessee CreateLessee()
        {
            return CreateLessee(1);
        }

        protected static Lessee CreateLessee(int borrowerId)
        {
            return new Lessee(borrowerId, "Hans", "Muster", "Strasse", "6000", "Luzern", "hans.muster@test.phundus.ch",
                "+4179123456", "");
        }

        protected static Lessee CreateLessee(int borrowerId, string firstName, string lastName, string street = "",
            string postcode = "", string city = "", string emailAddress = "", string mobilePhoneNumber = "",
            string memberNumber = "")
        {
            return new Lessee(borrowerId, firstName, lastName, street, postcode, city, emailAddress, mobilePhoneNumber,
                memberNumber);
        }
    }
}