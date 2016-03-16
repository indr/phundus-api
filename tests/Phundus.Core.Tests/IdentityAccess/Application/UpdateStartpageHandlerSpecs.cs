namespace Phundus.Tests.IdentityAccess.Application
{
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Model.Organizations;
    using Phundus.IdentityAccess.Organizations.Model;
    using Rhino.Mocks;

    [Subject(typeof (UpdateStartpageHandler))]
    public class when_handling_update_startpage :
        identityaccess_command_handler_concern<UpdateStartpage, UpdateStartpageHandler>
    {
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = new Organization(theOrganizationId, "The organization");
            depends.on<IOrganizationRepository>()
                .WhenToldTo(x => x.GetById(theOrganizationId.Id))
                .Return(theOrganization);

            command = new UpdateStartpage(theInitiatorId, theOrganizationId, "<p>New startpage</p>");
        };

        public It should_publish_organization_updated =
            () => publisher.WasToldTo(x => x.Publish(Arg<StartpageChanged>.Is.NotNull));
    }
}