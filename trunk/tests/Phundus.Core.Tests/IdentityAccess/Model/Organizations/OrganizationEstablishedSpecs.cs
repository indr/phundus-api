namespace Phundus.Tests.IdentityAccess.Model.Organizations
{
    using Events;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Organizations;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (OrganizationEstablished))]
    public class OrganizationEstablishedSpecs : identityaccess_domain_event_concern<OrganizationEstablished>
    {
        private static Founder theFounder;

        private Establish ctx = () =>
        {
            theFounder = new Founder(theInitiatorId, "founder@test.phundus.ch", "The Founder");
            sut_factory.create_using(() =>
                new OrganizationEstablished(theFounder, theOrganizationId, "Name", OrganizationPlan.Membership, true));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_founder = () =>
            dataMember(1).ShouldEqual(theFounder.ToActor());

        private It should_have_at_2_the_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_the_name = () =>
            dataMember(3).ShouldEqual("Name");

        private It should_have_at_4_the_organization_plan = () =>
            dataMember(4).ShouldEqual("membership");

        private It should_have_at_5_the_public_rental_setting = () =>
            dataMember(5).ShouldEqual(true);

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Organizations.Model.OrganizationEstablished");
    }
}