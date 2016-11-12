namespace Phundus.Tests.IdentityAccess.Events
{
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (OrganizationPlanChanged))]
    public class organization_plan_changed : identityaccess_domain_event_concern<OrganizationPlanChanged>
    {
        private Establish ctx = () => sut_factory.create_using(() =>
            new OrganizationPlanChanged(theAdmin, theOrganizationId, OrganizationPlan.Free, OrganizationPlan.Membership));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theAdmin.ToActor());

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