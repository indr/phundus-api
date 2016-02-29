namespace Phundus.Tests.IdentityAccess.Model.Organizations
{
    using Common.Domain.Model;
    using Events;
    using Integration.IdentityAccess;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Model;

    [Subject(typeof (OrganizationEstablished))]
    public class OrganizationEstablishSpecs : identityaccess_domain_event_concern<OrganizationEstablished>
    {
        private Establish ctx = () =>
        {
            var theFounder = new Founder(theInitiatorId, "founder@test.phundus.ch", "The Founder");
            sut_factory.create_using(() =>
                new OrganizationEstablished(theFounder, theOrganizationId, "Name", OrganizationPlan.Membership));
        };

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_founder = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_the_name = () =>
            dataMember(3).ShouldEqual("Name");

        private It should_have_at_4_the_organization_plan = () =>
            dataMember(4).ShouldEqual("membership");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Organizations.Model.OrganizationEstablished");
    }
}