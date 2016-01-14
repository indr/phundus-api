namespace Phundus.Tests.Shop.Orders.Model
{
    using Phundus.Shop.Contracts.Model;
    using Phundus.Shop.Orders.Model;

    public abstract class order_concern : concern<Order>
    {
        protected static Order order;
        protected static int modifierId = 101;

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