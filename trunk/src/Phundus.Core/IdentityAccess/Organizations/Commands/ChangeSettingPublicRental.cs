namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
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
        private readonly IMemberInRole _memberInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public ChangeSettingPublicRentalHandler(IMemberInRole memberInRole, IOrganizationRepository organizationRepository)
        {
            if (memberInRole == null) throw new ArgumentNullException("memberInRole");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _memberInRole = memberInRole;
            _organizationRepository = organizationRepository;
        }

        public void Handle(ChangeSettingPublicRental command)
        {
            var organization = _organizationRepository.GetById(command.OrganizationId);

            var initiator = _memberInRole.ActiveManager(organization.Id, command.InitiatorId);

            organization.ChangeSettingPublicRental(initiator, command.Value);
        }
    }
}