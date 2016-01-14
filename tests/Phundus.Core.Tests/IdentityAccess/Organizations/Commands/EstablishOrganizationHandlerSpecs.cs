namespace Phundus.Core.Tests.IdentityAccess.Organizations.Commands
{
    using System;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using Core.Inventory.Stores.Repositories;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Users.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (EstablishOrganizationHandler))]
    public class when_handled : handler_concern<EstablishOrganization, EstablishOrganizationHandler>
    {
        private static int theInititatorId = 1001;
        private static Guid theOrganizationGuid = Guid.NewGuid();
        private static IOrganizationRepository organizationRepository;
        private static IStoreRepository storeRepository;

        private Establish ctx = () =>
        {
            organizationRepository = depends.on<IOrganizationRepository>();
            depends.on<IUserRepository>()
                .WhenToldTo(x => x.GetById(theInititatorId))
                .Return(CreateAdmin(theInititatorId));
            command = new EstablishOrganization
            {
                InitiatorId = theInititatorId,
                Name = "New Organization",
                OrganizationId = theOrganizationGuid
            };
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
                     && p.OrganizationId == theOrganizationGuid
                     && p.Plan == "free"
                     && p.Url == "")));
        };
    }
}