namespace Phundus.Tests.IdentityAccess.Organizations.Commands
{
    using Common.Domain.Model;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Commands;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;
    using Phundus.IdentityAccess.Users.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (EstablishOrganizationHandler))]
    public class when_handled : identityaccess_handler_concern<EstablishOrganization, EstablishOrganizationHandler>
    {
        private static OrganizationId theOrganizationGuid = new OrganizationId();
        private static IOrganizationRepository organizationRepository;

        private Establish ctx = () =>
        {
            organizationRepository = depends.on<IOrganizationRepository>();
            depends.on<IUserRepository>()
                .WhenToldTo(x => x.GetByGuid(theInitiatorId))
                .Return(make.Admin(theInitiatorId));
            command = new EstablishOrganization(theInitiatorId, theOrganizationGuid, "New Organization");
        };

        private It should_add_to_organization_repository =
            () => organizationRepository.WasToldTo(x => x.Add(Arg<Organization>.Is.NotNull));

        private It should_publish_membership_application_approved =
            () => publisher.WasToldTo(x => x.Publish(Arg<MembershipApplicationApproved>.Is.NotNull));

        private It should_publish_membership_application_filed =
            () => publisher.WasToldTo(x => x.Publish(Arg<MembershipApplicationFiled>.Is.NotNull));

        private It should_publish_organization_established = () =>
        {
            publisher.WasToldTo(x => x.Publish(Arg<OrganizationEstablished>.Is.NotNull));
            publisher.WasToldTo(x => x.Publish(Arg<OrganizationEstablished>.Matches(
                p => p.Name == "New Organization"
                     && p.OrganizationId == theOrganizationGuid.Id
                     && p.Plan == "free"
                     && p.Url == "")));
        };
    }
}