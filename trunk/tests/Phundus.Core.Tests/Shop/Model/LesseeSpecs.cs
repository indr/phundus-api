namespace Phundus.Tests.Shop.Model
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.Shop.Model;

    [Subject(typeof (Lessee))]
    public class when_serializing_a_lessee : serialization_object_concern<Lessee>
    {
        private static LesseeId theLesseeId;
        private static string theMemberNumber;
        private static string theFirstName;
        private static string theLastName;
        private static string theStreet;
        private static string thePostcode;
        private static string theCity;
        private static string theEmailAddress;
        private static string theMobilePhoneNumber;

        private Establish ctx = () =>
        {
            theLesseeId = new LesseeId();
            theFirstName = "The first name";
            theLastName = "The last name";
            theStreet = "The street";
            thePostcode = "The postcode";
            theCity = "The city";
            theEmailAddress = "The email address";
            theMobilePhoneNumber = "The mobile phone number";
            theMemberNumber = "The member number";

            sut_factory.create_using(() =>
                new Lessee(theLesseeId, theFirstName, theLastName, theStreet, thePostcode, theCity, theEmailAddress,
                    theMobilePhoneNumber, theMemberNumber));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Model.Lessee");

        private It should_have_at_1_the_lessee_id = () =>
            dataMember(1).ShouldEqual(theLesseeId.Id);

        private It should_have_at_2_the_first_name = () =>
            dataMember(2).ShouldEqual(theFirstName);

        private It should_have_at_3_the_last_name = () =>
            dataMember(3).ShouldEqual(theLastName);

        private It should_have_at_4_the_street = () =>
            dataMember(4).ShouldEqual(theStreet);

        private It should_have_at_5_the_postcode = () =>
            dataMember(5).ShouldEqual(thePostcode);

        private It should_have_at_6_the_city = () =>
            dataMember(6).ShouldEqual(theCity);

        private It should_have_at_7_the_email_address = () =>
            dataMember(7).ShouldEqual(theEmailAddress);

        private It should_have_at_8_the_mobile_phone_number = () =>
            dataMember(8).ShouldEqual(theMobilePhoneNumber);

        private It should_have_at_9_the_member_number = () =>
            dataMember(9).ShouldEqual(theMemberNumber);

    }
}