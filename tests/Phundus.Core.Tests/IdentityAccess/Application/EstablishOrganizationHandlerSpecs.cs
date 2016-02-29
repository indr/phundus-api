﻿namespace Phundus.Tests.IdentityAccess.Application
{
    using developwithpassion.specifications.extensions;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Application;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Users.Repositories;
    using Rhino.Mocks;

    [Subject(typeof (EstablishOrganizationHandler))]
    public class when_handling_establish_organization :
        identityaccess_command_handler_concern<EstablishOrganization, EstablishOrganizationHandler>
    {
        private static string theName = "The organization name";

        private Establish ctx = () =>
        {
            depends.on<IUserRepository>()
                .WhenToldTo(x => x.GetById(theInitiatorId))
                .Return(make.User(theInitiatorId));
            userInRole.setup(x => x.Founder(theInitiatorId))
                .Return(new Founder(theInitiatorId, "founder@test.phundus.ch", "The Founder"));
            command = new EstablishOrganization(theInitiatorId, theOrganizationId, theName);
        };

        private It should_add_to_organization_repository = () =>
            organizationRepository.received(x => x.Add(Arg<Organization>.Is.NotNull));

        private It should_publish_membership_application_approved = () =>
            publisher.WasToldTo(x => x.Publish(Arg<MembershipApplicationApproved>.Is.NotNull));

        private It should_publish_membership_application_filed = () =>
            publisher.WasToldTo(x => x.Publish(Arg<MembershipApplicationFiled>.Is.NotNull));

        private It should_publish_organization_established = () =>
            published<OrganizationEstablished>(p =>
                Equals(p.Initiator, theInitiator)
                && p.OrganizationId == theOrganizationId.Id
                && p.Name == theName
                && p.Plan == "free"
                && p.PublicRental);
    }
}