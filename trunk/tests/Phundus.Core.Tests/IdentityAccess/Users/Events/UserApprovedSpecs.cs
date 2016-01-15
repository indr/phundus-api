namespace Phundus.Tests.IdentityAccess.Users.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (UserApproved))]
    public class when_instantiating_a_user_approved_event : domain_event_concern<UserApproved>
    {
        private static UserGuid theUserGuid = new UserGuid();
        private Because of = () => sut = new UserApproved(theInitiatorGuid, theUserGuid);

        private It should_have_the_initiator_guid_at_1 = () => dataMember(1).ShouldEqual(theInitiatorGuid.Id);

        private It should_have_the_user_guid_at_2 = () => dataMember(2).ShouldEqual(theUserGuid.Id);
    }
}