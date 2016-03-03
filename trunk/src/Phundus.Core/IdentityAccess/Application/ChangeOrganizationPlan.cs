namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
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

        public InitiatorId InitiatorId { get; set; }
        public OrganizationId OrganizationId { get; set; }
        public OrganizationPlan Plan { get; set; }
    }

    public class ChangeOrganizationPlanHandler : IHandleCommand<ChangeOrganizationPlan>
    {
        private readonly IUserInRole _userInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public ChangeOrganizationPlanHandler(IUserInRole userInRole, IOrganizationRepository organizationRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _userInRole = userInRole;
            _organizationRepository = organizationRepository;
        }

        public void Handle(ChangeOrganizationPlan command)
        {
            var admin = _userInRole.Admin(command.InitiatorId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.ChangePlan(admin, command.Plan);
        }
    }
}