﻿namespace Phundus.Tests.IdentityAccess.Organizations.Events
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (MembershipApplicationFiled))]
    public class when_instantiating_a_user_memberhsip_application_filed_event : domain_event_concern<MembershipApplicationFiled>
    {
        private static OrganizationGuid theOrganizationGuid = new OrganizationGuid();
        private static UserGuid theUserGuid = new UserGuid();


        private Because of = () => sut = new MembershipApplicationFiled(theInitiatorGuid, theOrganizationGuid, theUserGuid);

        private It should_have_the_initiator_guid_at_1 = () => dataMember(1).ShouldEqual(theInitiatorGuid.Id);

        private It should_have_the_organization_guid_at_2 = () => dataMember(2).ShouldEqual(theOrganizationGuid.Id);

        private It should_have_the_user_guid_at_3 = () => dataMember(3).ShouldEqual(theUserGuid.Id);
    }

    [Subject(typeof(MembershipApplicationRejected))]
    public class when_instantiating_a_membership_application_rejected_event : domain_event_concern<MembershipApplicationRejected>
    {
        private static OrganizationGuid theOrganizationGuid = new OrganizationGuid();
        private static UserGuid theUserGuid = new UserGuid();

        private Because of = () => sut = new MembershipApplicationRejected(theInitiatorGuid, theOrganizationGuid, theUserGuid);

        private It should_have_the_initiator_guid_at_1 = () => dataMember(1).ShouldEqual(theInitiatorGuid.Id);

        private It should_have_the_organization_guid_at_2 = () => dataMember(2).ShouldEqual(theOrganizationGuid.Id);

        private It should_have_the_user_guid_at_3 = () => dataMember(3).ShouldEqual(theUserGuid.Id);
    }

    [Subject(typeof(MembershipApplicationApproved))]
    public class when_instantiating_a_membership_application_approved_event : domain_event_concern<MembershipApplicationApproved>
    {
        private static OrganizationGuid theOrganizationGuid = new OrganizationGuid();
        private static UserGuid theUserGuid = new UserGuid();

        private Because of = () => sut = new MembershipApplicationApproved(theInitiatorGuid, theOrganizationGuid, theUserGuid);

        private It should_have_the_initiator_guid_at_1 = () => dataMember(1).ShouldEqual(theInitiatorGuid.Id);

        private It should_have_the_organization_guid_at_2 = () => dataMember(2).ShouldEqual(theOrganizationGuid.Id);

        private It should_have_the_user_guid_at_3 = () => dataMember(3).ShouldEqual(theUserGuid.Id);
    }
}