namespace Phundus.Common.Tests.Domain.Model
{
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class postal_address_factory_concern : Observes
    {
        protected static string firstName;
        protected static string lastName;
        protected static string street;
        protected static string postcode;
        protected static string city;

        protected static string result;

        private Establish ctx = () =>
        {
            firstName = "first";
            lastName = "last";
            street = "street";
            postcode = "postcode";
            city = "city";
        };

        private Because of = () =>
            result = PostalAddressFactory.Make(firstName, lastName, street, postcode, city);
    }

    public class when_generating_with_every_field_present : postal_address_factory_concern
    {
        private It should_return = () =>
            result.ShouldEqual("first last\r\nstreet\r\npostcode city\r\n");
    }
}