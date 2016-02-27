namespace Phundus.Tests.Shop.Events
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public class shop_domain_event_concern<TDomainEvent> : domain_event_concern<TDomainEvent> where TDomainEvent : class
    {
        protected static Lessor theLessor;
        protected static Lessee theLessee;

        private Establish ctx = () =>
        {
            theLessor = new Lessor(new LessorId(), "The lessor", true);
            theLessee = new Lessee(new LesseeId(), "First name", "Last name", "Street", "Postcode", "City",
                "Email address", "Phone number", "Member number");
        };

        protected static OrderEventItem CreateOrderEventItem()
        {
            return new OrderEventItem(new OrderLineId(), new ArticleId(), new ArticleShortId(1234),
                "The text", 1.23m, Period.FromNow(1), 10, 12.3m);
        }
    }
}