namespace Phundus.IdentityAccess.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Organizations;

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
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserInRole _userInRole;

        public ChangeSettingPublicRentalHandler(IUserInRole userInRole, IOrganizationRepository organizationRepository)
        {            
            _userInRole = userInRole;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(ChangeSettingPublicRental command)
        {
            var manager = _userInRole.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.ChangeSettingPublicRental(manager, command.Value);
        }
    }
}