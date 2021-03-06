﻿namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Organizations;
    using Organizations.Model;
    using Users.Services;

    public class ChangeOrganizationPlan : ICommand
    {
        public ChangeOrganizationPlan(InitiatorId initiatorId, OrganizationId organizationId, string plan)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (plan == null) throw new ArgumentNullException("plan");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;

            plan = plan.ToLowerInvariant();
            if (plan == "free")
                Plan = OrganizationPlan.Free;
            else if (plan == "membership")
                Plan = OrganizationPlan.Membership;
            else
                throw new ArgumentOutOfRangeException("plan");
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public OrganizationPlan Plan { get; protected set; }
    }

    public class ChangeOrganizationPlanHandler : IHandleCommand<ChangeOrganizationPlan>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserInRoleService _userInRoleService;

        public ChangeOrganizationPlanHandler(IUserInRoleService userInRoleService, IOrganizationRepository organizationRepository)
        {
            if (userInRoleService == null) throw new ArgumentNullException("userInRoleService");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _userInRoleService = userInRoleService;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(ChangeOrganizationPlan command)
        {
            var admin = _userInRoleService.Admin(command.InitiatorId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.ChangePlan(admin, command.Plan);
        }
    }
}