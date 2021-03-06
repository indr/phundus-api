﻿namespace Phundus.Tests.IdentityAccess.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Model;

    [Subject(typeof (UserApproved))]
    public class when_instantiating_a_user_approved_event : domain_event_concern<UserApproved>
    {
        private static UserId theUserGuid = new UserId();

        private Establish ctx = () => sut_factory.create_using(() =>
            new UserApproved(theInitiatorId, theUserGuid));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Users.Model.UserApproved");

        private It should_have_the_initiator_guid_at_1 = () =>
            dataMember(1).ShouldEqual(theInitiatorId.Id);

        private It should_have_the_user_guid_at_2 = () =>
            dataMember(2).ShouldEqual(theUserGuid.Id);
    }
}