namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Common;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;

    public class EstablishOrganization
    {
        public int InitiatorId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
    }

    public class EstablishOrganizationHandler : IHandleCommand<EstablishOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public EstablishOrganizationHandler(IOrganizationRepository organizationRepository)
        {
            AssertionConcern.AssertArgumentNotNull(organizationRepository, "OrganizationRepository must be provided.");

            _organizationRepository = organizationRepository;
        }

        public void Handle(EstablishOrganization command)
        {
            var organization = new Organization(command.Name, command.InitiatorId);

            _organizationRepository.Add(organization);

            EventPublisher.Publish(new OrganizationEstablished(organization.Id, organization.Name, organization.Url, command.InitiatorId));

            command.OrganizationId = organization.Id;
        }
    }
}