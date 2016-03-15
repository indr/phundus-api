namespace Phundus.Tests.Shop.Model
{
    using Common.Domain.Model;
    using Common.Tests;
    using Machine.Specifications;
    using Phundus.Shop.Model;

    [Subject(typeof (Lessor))]
    public class when_serializing_a_lessor : serialization_object_concern<Lessor>
    {
        private static LessorId theLessorId = new LessorId();
        private static string theName = "name";
        private static string thePostalAddress = "Street\n10AB City";
        private static string thePhoneNumber = "012 345 67 89";
        private static string theEmailAddress = "lessor@test.phundus.ch";
        private static string theWebsite = "https://the.website";
        private static bool theDoesPublicRental = true;

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new Lessor(theLessorId, theName, thePostalAddress, thePhoneNumber, theEmailAddress, theWebsite, theDoesPublicRental));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_lessor_id = () =>
            dataMember(1).ShouldEqual(theLessorId.Id);

        private It should_have_at_2_the_name = () =>
            dataMember(2).ShouldEqual(theName);

        private It should_have_at_3_the_does_public_rental = () =>
            dataMember(3).ShouldEqual(theDoesPublicRental);

        private It should_have_at_4_the_postal_address = () =>
            dataMember(4).ShouldEqual(thePostalAddress);

        private It should_have_at_5_the_phone_number = () =>
            dataMember(5).ShouldEqual(thePhoneNumber);

        private It should_have_at_6_the_email_address = () =>
            dataMember(6).ShouldEqual(theEmailAddress);

        private It should_have_at_7_the_website = () =>
            dataMember(7).ShouldEqual(theWebsite);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.Shop.Model.Lessor");
    }
}