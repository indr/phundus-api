﻿namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System;
    using Common;
    using Cqrs;
    using Ddd;
    using IdentityAccess.Users.Repositories;
    using Model;
    using Repositories;

    public class EstablishOrganization
    {
        public int InitiatorId { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
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
            var organization = new Organization(command.OrganizationId, command.Name);

            _organizationRepository.Add(organization);

            EventPublisher.Publish(new OrganizationEstablished(organization.Id, organization.Plan, organization.Name,
                organization.Url));

            var requestId = Guid.NewGuid();
            var user = _userRepository.FindById(command.InitiatorId);
            var application = organization.RequestMembership(requestId, user);

            var membershipId = Guid.NewGuid();
            organization.ApproveMembershipRequest(application, membershipId);
            organization.SetMembersRole(user, Role.Chief);
        }
    }
}