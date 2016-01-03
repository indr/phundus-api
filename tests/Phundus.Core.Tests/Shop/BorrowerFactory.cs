namespace Phundus.Core.Tests.Shop
{
    using Core.Shop.Contracts.Model;

    public class BorrowerFactory
    {
        public static Lessee Create()
        {
            return Create(1);
        }

        public static Lessee Create(int borrowerId)
        {
            return new Lessee(borrowerId, "Hans", "Muster", "Strasse", "6000", "Luzern", "hans.muster@test.phundus.ch",
                "+4179123456", "");
        }

        public static Lessee Create(int borrowerId, string firstName, string lastName, string street = "",
            string postcode = "", string city = "", string emailAddress = "", string mobilePhoneNumber = "",
            string memberNumber = "")
        {
            return new Lessee(borrowerId, firstName, lastName, street, postcode, city, emailAddress, mobilePhoneNumber,
                memberNumber);
        }
    }
}