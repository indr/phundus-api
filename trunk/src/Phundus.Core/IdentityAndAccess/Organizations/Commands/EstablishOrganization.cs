namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;
    using Users.Model;
    using Users.Repositories;
    using Role = Model.Role;

    public class EstablishOrganization
    {
        public int InitiatorId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
    }

    public class EstablishOrganizationHandler : IHandleCommand<EstablishOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private IUserRepository _userRepository;

        public EstablishOrganizationHandler(IOrganizationRepository organizationRepository, IUserRepository userRepository)
        {
            AssertionConcern.AssertArgumentNotNull(organizationRepository, "OrganizationRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(userRepository, "UserRepository must be provided.");

            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        public void Handle(EstablishOrganization command)
        {
            var organization = new Organization(command.Name);

            _organizationRepository.Add(organization);

            EventPublisher.Publish(new OrganizationEstablished(organization.Id, organization.Name, organization.Url, command.InitiatorId));

            var requestId = Guid.NewGuid();
            var user = _userRepository.FindById(command.InitiatorId);
            var application = organization.RequestMembership(requestId, user);

            var membershipId = Guid.NewGuid();
            organization.ApproveMembershipRequest(application, membershipId);
            organization.SetMembersRole(user, Role.Chief);

            command.OrganizationId = organization.Id;
        }
    }
}