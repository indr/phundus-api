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
    public class when_update_organization_details_is_handled : organization_handler_concern
    {
        private static IMemberInRole memberInRole;
        private static IOrganizationRepository repository;
        private static Organization theOrganization;

        private Establish ctx = () =>
        {
            theOrganization = make.Organization();
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IOrganizationRepository>();

            repository.setup(x => x.GetById(theOrganization.Id)).Return(theOrganization);

            command = new ChangeOrganizationContactDetails(theInitiatorId, new OrganizationGuid(theOrganization.Id),
                "New post address", "New phone number", "New email address", "New website");
        };

        private It should_ask_for_chief_privileges = () =>
            memberInRole.WasToldTo(x => x.ActiveChief(theOrganization.Id, theInitiatorId));

        private It should_tell_organization_to_change_contact_details = () =>
            theOrganization.WasToldTo(x => x.ChangeContactDetails(Arg<ContactDetails>.Is.NotNull));
    }
}