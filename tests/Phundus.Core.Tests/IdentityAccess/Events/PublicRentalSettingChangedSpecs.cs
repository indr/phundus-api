namespace Phundus.Tests.IdentityAccess.Events
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Organizations.Model;

    public class identityaccess_domain_event_concern<TDomainEvent> : domain_event_concern<TDomainEvent>
        where TDomainEvent : class
    {
        protected static Admin theAdmin;
        protected static Manager theManager;
        protected static OrganizationId theOrganizationId = new OrganizationId();

        private Establish ctx = () =>
        {
            var make = new identityaccess_factory(fake);
            theAdmin = make.Admin(theInitiatorId);
            theManager = make.Manager(theInitiatorId);
        };
    }

    [Subject(typeof (PublicRentalSettingChanged))]
    public class public_rental_setting_changed : identityaccess_domain_event_concern<PublicRentalSettingChanged>
    {
        private static bool theValue = true;

        private Establish ctx = () => sut_factory.create_using(() =>
            new PublicRentalSettingChanged(theManager, theOrganizationId, theValue));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_initiator = () =>
            dataMember(1).ShouldEqual(theManager);

        private It should_have_at_2_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_value = () =>
            dataMember(3).ShouldEqual(theValue);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.PublicRentalSettingChanged");
    }

    [Subject(typeof (OrganizationPlanChanged))]
    public class organization_plan_changed : identityaccess_domain_event_concern<OrganizationPlanChanged>
    {
        private Establish ctx = () => sut_factory.create_using(() =>
            new OrganizationPlanChanged(theAdmin, theOrganizationId, OrganizationPlan.Free, OrganizationPlan.Membership));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_the_old_plan = () =>
            dataMember(3).ShouldEqual("free");

        private It should_have_at_4_the_new_plan = () =>
            dataMember(4).ShouldEqual("membership");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.OrganizationPlanChanged");
    }
}