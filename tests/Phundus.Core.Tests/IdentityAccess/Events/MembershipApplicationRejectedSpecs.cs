﻿namespace Phundus.Tests.IdentityAccess.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (MembershipApplicationRejected))]
    public class when_instantiating_a_membership_application_rejected_event :
        domain_event_concern<MembershipApplicationRejected>
    {
        private static OrganizationId theOrganizationId = new OrganizationId();
        private static UserId theUserId = new UserId();

        private Establish ctx = () => sut_factory.create_using(() =>
            new MembershipApplicationRejected(theInitiatorId, theOrganizationId, theUserId));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Organizations.Model.MembershipApplicationRejected");

        private It should_have_an_int_at_1 = () =>
            dataMember(1).ShouldBeOfExactType<int>();

        private It should_have_an_int_at_2 = () =>
            dataMember(2).ShouldBeOfExactType<int>();

        private It should_have_the_initiator_guid_at_4 = () =>
            dataMember(4).ShouldEqual(theInitiatorId.Id);

        private It should_have_the_organization_guid_at_3 = () =>
            dataMember(3).ShouldEqual(theOrganizationId.Id);

        private It should_have_the_user_guid_at_5 = () =>
            dataMember(5).ShouldEqual(theUserId.Id);
    }
}