namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;
    using Model;
    using Repositories;
    using Users.Repositories;

    public class EstablishOrganization
    {
        public EstablishOrganization(InitiatorId initiatorId, OrganizationGuid organizationGuid, string name)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationGuid == null) throw new ArgumentNullException("organizationGuid");
            if (name == null) throw new ArgumentNullException("name");
            InitiatorId = initiatorId;
            OrganizationGuid = organizationGuid;
            Name = name;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationGuid OrganizationGuid { get; protected set; }
        public string Name { get; protected set; }
    }

    public class EstablishOrganizationHandler : IHandleCommand<EstablishOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        public EstablishOrganizationHandler(IOrganizationRepository organizationRepository,
            IUserRepository userRepository)
        {
            AssertionConcern.AssertArgumentNotNull(organizationRepository, "OrganizationRepository must be provided.");
            AssertionConcern.AssertArgumentNotNull(userRepository, "UserRepository must be provided.");

            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        public void Handle(EstablishOrganization command)
        {
            var organization = new Organization(command.InitiatorId, command.OrganizationGuid, command.Name);

            _organizationRepository.Add(organization);

            EventPublisher.Publish(new OrganizationEstablished(organization.Id,
                organization.Plan.ToString().ToLowerInvariant(), organization.Name,
                organization.Url));

            var requestId = Guid.NewGuid();
            var user = _userRepository.GetByGuid(command.InitiatorId);
            var application = organization.RequestMembership(command.InitiatorId, requestId, user);

            var membershipId = Guid.NewGuid();
            organization.ApproveMembershipRequest(command.InitiatorId, application, membershipId);
            organization.SetMembersRole(user, Role.Chief);
        }
    }
}