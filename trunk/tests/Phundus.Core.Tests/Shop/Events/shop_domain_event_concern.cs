namespace Phundus.Tests.Shop.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public class shop_domain_event_concern<TDomainEvent> : domain_event_concern<TDomainEvent> where TDomainEvent : class
    {
        protected static Manager theManager;
        protected static Lessor theLessor;
        protected static Lessee theLessee;

        private Establish ctx = () =>
        {
            theManager = new Manager(new UserId(), "manager@test.phundus.ch", "The Manager");
            theLessor = new Lessor(new LessorId(), "The lessor", "postalAddress", "phoneNumber", "emailAddress", "website", true);
            theLessee = new Lessee(new LesseeId(), "First name", "Last name", "Street", "Postcode", "City",
                "Email address", "Phone number", "Member number");
        };

        protected static OrderEventLine CreateOrderEventItem()
        {
            return new OrderEventLine(new OrderLineId(), new ArticleId(), new ArticleShortId(1234),
                new StoreId(), "The text", 1.23m, Period.FromNow(1), 10, 12.3m);
        }
    }
}