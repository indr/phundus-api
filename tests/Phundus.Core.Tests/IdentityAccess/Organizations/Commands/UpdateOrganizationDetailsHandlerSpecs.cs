namespace Phundus.Tests.IdentityAccess.Organizations.Commands
{
    using System;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.IdentityAccess.Organizations.Commands;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;
    using Phundus.IdentityAccess.Queries;
    using Rhino.Mocks;

    [Subject(typeof (UpdateOrganizationDetailsHandler))]
    public class when_update_organization_details_is_handled :
        handler_concern<UpdateOrganizationDetails, UpdateOrganizationDetailsHandler>
    {
        protected static IMemberInRole memberInRole;
        protected static IOrganizationRepository repository;

        protected static Guid organizationId;
        protected static int userId = 2;
        private static Organization organization;

        protected Establish c = () =>
        {
            organizationId = Guid.NewGuid();
            organization = new Organization(organizationId, "Organization");
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IOrganizationRepository>();

            repository.setup(x => x.GetById(organizationId)).Return(organization);

            command = new UpdateOrganizationDetails
            {
                OrganizationId = organizationId,
                InitiatorId = userId
            };
        };

        public It should_ask_for_chief_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveChief(organizationId, userId));

        public It should_publish_organization_updated =
            () => publisher.WasToldTo(x => x.Publish(Arg<OrganizationUpdated>.Is.NotNull));
    }
}