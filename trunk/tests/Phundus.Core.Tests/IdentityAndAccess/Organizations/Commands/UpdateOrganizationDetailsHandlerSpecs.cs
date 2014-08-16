namespace Phundus.Core.Tests.IdentityAndAccess
{
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using Core.IdentityAndAccess.Queries;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (UpdateOrganizationDetailsHandler))]
    public class when_update_organization_details_is_handled :
        handler_concern<UpdateOrganizationDetails, UpdateOrganizationDetailsHandler>
    {
        protected static IMemberInRole memberInRole;
        protected static IOrganizationRepository repository;

        protected static int organizationId = 1;
        protected static int userId = 2;
        private static readonly Organization organization = new Organization(organizationId);

        protected Establish c = () =>
        {
            memberInRole = depends.on<IMemberInRole>();
            repository = depends.on<IOrganizationRepository>();

            repository.setup(x => x.ById(organizationId)).Return(organization);

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