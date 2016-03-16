namespace Phundus.Tests.IdentityAccess.Application
{
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Organizations.Model;
    using Rhino.Mocks;

    [Subject(typeof (ChangeOrganizationContactDetailsHandler))]
    public class when_update_organization_details_is_handled :
        identityaccess_command_handler_concern
            <ChangeOrganizationContactDetails, ChangeOrganizationContactDetailsHandler>
    {
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization(theOrganizationId);

            organizationRepository.setup(x => x.GetById(theOrganization.Id)).Return(theOrganization);

            command = new ChangeOrganizationContactDetails(theInitiatorId, theOrganization.Id,
                "New line 1", "New line 2", "New street", "New postcode", "New city", "New phone number",
                "New email address", "New website");
        };

        private It should_change_rganizations_contact_details = () =>
            theOrganization.WasToldTo(x => x.ChangeContactDetails(Arg<Manager>.Is.Same(theManager), Arg<ContactDetails>.Is.NotNull));
    }
}