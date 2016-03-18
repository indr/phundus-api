namespace Phundus.Tests.IdentityAccess.Model.Users
{
    using Common.Domain.Model;
    using IdentityAccess.Events;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;

    [Subject(typeof (UserAddressChanged))]
    public class UserAddressChangedSpecs : identityaccess_domain_event_concern<UserAddressChanged>
    {
        private static UserId theUserId;
        private static string theFirstName;
        private static string theLastName;
        private static string theStreet;
        private static string thePostcode;
        private static string theCity;
        private static string thePhoneNumber;

        private Establish ctx = () =>
        {
            theUserId = new UserId();
            theFirstName = "The first name";
            theLastName = "The last name";
            theStreet = "The street";
            thePostcode = "The postcode";
            theCity = "The city";
            thePhoneNumber = "The phone number";

            sut_factory.create_using(() =>
                new UserAddressChanged(theInitiator, theUserId, theFirstName, theLastName, theStreet, thePostcode,
                    theCity, thePhoneNumber));
        };

        private It shold_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.Users.UserAddressChanged");

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator.ToActor());

        private It should_have_at_2_the_user_id = () =>
            dataMember(2).ShouldEqual(theUserId.Id);

        private It should_have_at_3_the_first_name = () =>
            dataMember(3).ShouldEqual(theFirstName);

        private It should_have_at_4_the_last_name = () =>
            dataMember(4).ShouldEqual(theLastName);

        private It should_have_at_5_the_street = () =>
            dataMember(5).ShouldEqual(theStreet);

        private It should_have_at_6_the_postcode = () =>
            dataMember(6).ShouldEqual(thePostcode);

        private It should_have_at_7_the_city = () =>
            dataMember(7).ShouldEqual(theCity);

        private It should_have_at_8_the_phone_number = () =>
            dataMember(8).ShouldEqual(thePhoneNumber);
    }
}