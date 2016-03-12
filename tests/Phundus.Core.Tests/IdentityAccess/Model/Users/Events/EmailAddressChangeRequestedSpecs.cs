namespace Phundus.Tests.IdentityAccess.Model.Users.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;

    [Subject(typeof (EmailAddressChangeRequested))]
    public class EmailAddressChangeRequestedSpecs : domain_event_concern<EmailAddressChangeRequested>
    {
        private static UserId theUserId = new UserId();
        private static string theFirstName = "First";
        private static string theLastName = "Last";
        private static string theRequestedEmailAddress = "requested@test.phundus.ch";
        private static string theValidationKey = "validationKey";

        private Establish ctx = () => sut_factory.create_using(() =>
            new EmailAddressChangeRequested(theInitiator, theUserId, theFirstName, theLastName,
                theRequestedEmailAddress, theValidationKey));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_user_id = () =>
            dataMember(2).ShouldEqual(theUserId.Id);

        private It should_have_at_3_the_first_name = () =>
            dataMember(3).ShouldEqual(theFirstName);

        private It should_have_at_4_the_last_name = () =>
            dataMember(4).ShouldEqual(theLastName);

        private It should_have_at_5_the_requested_email_address = () =>
            dataMember(5).ShouldEqual(theRequestedEmailAddress);

        private It should_have_at_6_the_validation_key = () =>
            dataMember(6).ShouldEqual(theValidationKey);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.Users.EmailAddressChangeRequested");
    }
}