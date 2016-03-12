namespace Phundus.Tests.IdentityAccess.Model.Users
{
    using Common.Domain.Model;
    using IdentityAccess.Events;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (UserEmailAddressChanged))]
    public class UserEmailAddressChangedSpecs : identityaccess_domain_event_concern<UserEmailAddressChanged>
    {
        private static UserId theUserId = new UserId();
        private static string theOldEmailAddress = "old@test.phundus.ch";
        private static string theNewEmailAddress = "new@test.phundus.ch";

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new UserEmailAddressChanged(theInitiator, theUserId, theOldEmailAddress, theNewEmailAddress));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_user_id = () =>
            dataMember(1).ShouldEqual(theUserId.Id);

        private It should_have_at_2_the_old_email_address = () =>
            dataMember(2).ShouldEqual(theOldEmailAddress);

        private It should_have_at_3_the_new_email_address = () =>
            dataMember(3).ShouldEqual(theNewEmailAddress);

        private It should_have_at_4_the_initiator = () =>
            dataMember(4).ShouldEqual(theInitiator);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Users.Model.UserEmailAddressChanged");
    }
}