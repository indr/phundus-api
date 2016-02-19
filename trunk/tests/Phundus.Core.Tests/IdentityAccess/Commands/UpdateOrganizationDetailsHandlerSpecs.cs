namespace Phundus.Tests.IdentityAccess.Organizations.Commands
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Commands;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;
    using Phundus.IdentityAccess.Queries;
    using Rhino.Mocks;

    [Subject(typeof (ChangeOrganizationContactDetailsHandler))]
    public class when_update_organization_details_is_handled : identityaccess_command_handler_concern<ChangeOrganizationContactDetails, ChangeOrganizationContactDetailsHandler>
    {
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            

            organizationRepository.setup(x => x.GetById(theOrganization.Id)).Return(theOrganization);

            command = new ChangeOrganizationContactDetails(theInitiatorId, theOrganization.Id,
                "New post address", "New phone number", "New email address", "New website");
        };

        private It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveManager(theOrganization.Id.Id, theInitiatorId));

        private It should_tell_organization_to_change_contact_details = () =>
            theOrganization.WasToldTo(x => x.ChangeContactDetails(Arg<ContactDetails>.Is.NotNull));
    }
}