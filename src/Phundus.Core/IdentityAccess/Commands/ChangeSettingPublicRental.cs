﻿namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using Phundus.Authorization;
    using Queries;
    using Repositories;

    public class ChangeSettingPublicRental : ICommand
    {
        public ChangeSettingPublicRental(InitiatorId initiatorId, OrganizationId organizationId, bool value)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            Value = value;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public bool Value { get; protected set; }
    }

    public class ChangeSettingPublicRentalHandler : IHandleCommand<ChangeSettingPublicRental>
    {
        private readonly IAuthorize _authorize;
        private readonly IInitiatorService _initiatorService;
        private readonly IOrganizationRepository _organizationRepository;

        public ChangeSettingPublicRentalHandler(IAuthorize authorize, IInitiatorService initiatorService, IOrganizationRepository organizationRepository)
        {
            if (authorize == null) throw new ArgumentNullException("authorize");
            if (initiatorService == null) throw new ArgumentNullException("initiatorService");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _authorize = authorize;
            _initiatorService = initiatorService;
            _organizationRepository = organizationRepository;
        }

        public void Handle(ChangeSettingPublicRental command)
        {
            var organization = _organizationRepository.GetById(command.OrganizationId);
            var initiator = _initiatorService.GetActiveById(command.InitiatorId);

            _authorize.User(initiator.InitiatorId, Manage.Organization(organization.Id));
            

            organization.ChangeSettingPublicRental(initiator, command.Value);
        }
    }
}