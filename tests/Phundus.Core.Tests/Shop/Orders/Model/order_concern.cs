namespace Phundus.Tests.Shop.Orders.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public abstract class order_concern : creating_order_concern
    {
        protected static shop_factory make;

        private Establish ctx = () =>
        {
            make = new shop_factory(fake);
            theLessor = new Lessor(new LessorId(), "The lessor", false);
            theLessee = CreateLessee();
            sut_factory.create_using(() => new Order(theLessor, theLessee));
        };
    }

    public abstract class creating_order_concern : aggregate_concern_new<Order>
    {
        protected static InitiatorId theInitiatorId;
        protected static Lessor theLessor;
        protected static Lessee theLessee;

        private Establish ctx = () => theInitiatorId = new InitiatorId();

        protected static Article CreateArticle(int articleId, Owner theOwner)
        {
            return new Article(articleId, new ArticleId(), theOwner, "Article " + articleId, 7.0m);
        }

        protected static Lessee CreateLessee()
        {
            return CreateLessee(new LesseeId());
        }

        protected static Lessee CreateLessee(LesseeId borrowerId)
        {
            return new Lessee(borrowerId, "Hans", "Muster", "Strasse", "6000", "Luzern", "hans.muster@test.phundus.ch",
                "+4179123456", "");
        }

        protected static Lessee CreateLessee(Guid borrowerId, string firstName, string lastName, string street = "",
            string postcode = "", string city = "", string emailAddress = "", string mobilePhoneNumber = "",
            string memberNumber = "")
        {
            return new Lessee(new LesseeId(borrowerId), firstName, lastName, street, postcode, city, emailAddress, mobilePhoneNumber,
                memberNumber);
        }
    }
}