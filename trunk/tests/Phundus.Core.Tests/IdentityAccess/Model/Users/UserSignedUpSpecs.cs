namespace Phundus.Tests.IdentityAccess.Model.Users
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (UserSignedUp))]
    public class UserSignedUpSpecs : domain_event_concern<UserSignedUp>
    {
        private static UserId theUserId = new UserId();
        private static UserShortId theUserShortId = new UserShortId(1234);
        private static string theEmailAddress = "The mail address";
        private static string thePassword = "The password";
        private static string theSalt = "The salt";
        private static string theValidationKey = "The validation key";
        private static string theFirstName = "The first name";
        private static string theLastName = "The last name";
        private static string theStreet = "The street";
        private static string thePostcode = "The postcode";
        private static string theCity = "The city";
        private static string thePhoneNumber = "The phone number";

        private Establish ctx = () => sut_factory.create_using(() =>
            new UserSignedUp(theUserId, theUserShortId, theEmailAddress, thePassword, theSalt, theValidationKey, theFirstName,
                theLastName, theStreet, thePostcode, theCity, thePhoneNumber));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_10_the_city = () =>
            dataMember(10).ShouldEqual(theCity);

        private It should_have_at_11_the_phone_number = () =>
            dataMember(11).ShouldEqual(thePhoneNumber);

        private It should_have_at_12_an_empty_int_as_js_number = () =>
            dataMember(12).ShouldEqual(null);

        private It should_have_at_13_the_user_id = () =>
            dataMember(13).ShouldEqual(theUserId.Id);

        private It should_have_at_1_the_user_short_id = () =>
            dataMember(1).ShouldEqual(theUserShortId.Id);

        private It should_have_at_2_the_email_address = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_at_3_the_password = () =>
            dataMember(3).ShouldEqual(thePassword);

        private It should_have_at_4_the_salt = () =>
            dataMember(4).ShouldEqual(theSalt);

        private It should_have_at_5_the_validation_key = () =>
            dataMember(5).ShouldEqual(theValidationKey);

        private It should_have_at_6_the_first_name = () =>
            dataMember(6).ShouldEqual(theFirstName);

        private It should_have_at_7_the_last_name = () =>
            dataMember(7).ShouldEqual(theLastName);

        private It should_have_at_8_the_street = () =>
            dataMember(8).ShouldEqual(theStreet);

        private It should_have_at_9_the_postcode = () =>
            dataMember(9).ShouldEqual(thePostcode);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Users.Model.UserSignedUp");
    }
}