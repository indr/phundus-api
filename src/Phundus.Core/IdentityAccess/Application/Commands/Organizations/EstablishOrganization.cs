﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Eventing;
    using Model;
    using Model.Organizations;
    using Model.Users;
    using Organizations.Model;

    public class EstablishOrganization : ICommand
    {
        public EstablishOrganization(InitiatorId initiatorId, OrganizationId organizationId, string name)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (name == null) throw new ArgumentNullException("name");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            Name = name;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public string Name { get; protected set; }
    }

    public class EstablishOrganizationHandler : IHandleCommand<EstablishOrganization>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserInRoleService _userInRoleService;
        private readonly IUserRepository _userRepository;

        public EstablishOrganizationHandler(IUserInRoleService userInRoleService, IOrganizationRepository organizationRepository,
            IUserRepository userRepository)
        {            
            _userInRoleService = userInRoleService;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        [Transaction]
        public void Handle(EstablishOrganization command)
        {
            var founder = _userInRoleService.Founder(command.InitiatorId);
            var organization = new Organization(command.OrganizationId, command.Name);

            _organizationRepository.Add(organization);

            EventPublisher.Publish(new OrganizationEstablished(founder, organization.Id,
                organization.Name, organization.Plan, organization.Settings.PublicRental));

            var requestId = new MembershipApplicationId();
            var user = _userRepository.GetById(command.InitiatorId);
            var application = organization.ApplyForMembership(command.InitiatorId, requestId, user);

            var membershipId = Guid.NewGuid();
            organization.ApproveMembershipApplication(command.InitiatorId, application, membershipId);
            organization.SetMembersRole(user, MemberRole.Manager);
        }
    }
}