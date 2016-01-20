namespace Phundus.Tests.IdentityAccess.Organizations.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (MembershipApplicationFiled))]
    public class when_instantiating_a_user_memberhsip_application_filed_event :
        domain_event_concern<MembershipApplicationFiled>
    {
        private static OrganizationId theOrganizationId = new OrganizationId();
        private static UserId theUserId = new UserId();


        private Because of = () => sut = new MembershipApplicationFiled(theInitiatorId, theOrganizationId, theUserId);

        private It should_have_the_initiator_guid_at_4 = () => dataMember(4).ShouldEqual(theInitiatorId.Id);

        private It should_have_the_organization_guid_at_3 = () => dataMember(3).ShouldEqual(theOrganizationId.Id);

        private It should_have_the_user_guid_at_5 = () => dataMember(5).ShouldEqual(theUserId.Id);
    }

    [Subject(typeof (MembershipApplicationRejected))]
    public class when_instantiating_a_membership_application_rejected_event :
        domain_event_concern<MembershipApplicationRejected>
    {
        private static OrganizationId theOrganizationId = new OrganizationId();
        private static UserId theUserId = new UserId();

        private Because of = () => sut = new MembershipApplicationRejected(theInitiatorId, theOrganizationId, theUserId);

        private It should_have_the_initiator_guid_at_4 = () => dataMember(4).ShouldEqual(theInitiatorId.Id);

        private It should_have_the_organization_guid_at_3 = () => dataMember(3).ShouldEqual(theOrganizationId.Id);

        private It should_have_the_user_guid_at_5 = () => dataMember(5).ShouldEqual(theUserId.Id);
    }

    [Subject(typeof (MembershipApplicationApproved))]
    public class when_instantiating_a_membership_application_approved_event :
        domain_event_concern<MembershipApplicationApproved>
    {
        private static OrganizationId theOrganizationId = new OrganizationId();
        private static UserId theUserId = new UserId();

        private Because of = () => sut = new MembershipApplicationApproved(theInitiatorId, theOrganizationId, theUserId);

        private It should_have_the_initiator_guid_at_4 = () => dataMember(4).ShouldEqual(theInitiatorId.Id);

        private It should_have_the_organization_guid_at_3 = () => dataMember(3).ShouldEqual(theOrganizationId.Id);

        private It should_have_the_user_guid_at_5 = () => dataMember(5).ShouldEqual(theUserId.Id);
    }
}