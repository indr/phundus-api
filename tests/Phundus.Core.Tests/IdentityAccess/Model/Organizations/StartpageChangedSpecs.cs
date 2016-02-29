namespace Phundus.Tests.IdentityAccess.Model.Organizations
{
    using Events;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Organizations;

    [Subject(typeof (StartpageChanged))]
    public class StartpageChangedSpecs : identityaccess_domain_event_concern<StartpageChanged>
    {
        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new StartpageChanged(theInitiator, theOrganizationId, "The new startpage"));

        private It should_be_in_assembly = () =>
            itsAssembly.ShouldEqual("Phundus.Core");

        private It should_have_at_1_the_initiator = () =>
            dataMember(1).ShouldEqual(theInitiator);

        private It should_have_at_2_the_organization_id = () =>
            dataMember(2).ShouldEqual(theOrganizationId.Id);

        private It should_have_at_3_the_startpage = () =>
            dataMember(3).ShouldEqual("The new startpage");

        private It should_have_full_name = () =>
            itsFullName.ShouldEqual("Phundus.IdentityAccess.Model.Organizations.StartpageChanged");
    }
}