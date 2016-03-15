namespace Phundus.IdentityAccess.Application
{
    using System;
    using Authorization;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Organizations;
    using Phundus.Authorization;
    using Resources;
    using Users.Services;

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
        private readonly IUserInRole _userInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public ChangeSettingPublicRentalHandler(IAuthorize authorize, IUserInRole userInRole, IOrganizationRepository organizationRepository)
        {
            _authorize = authorize;
            _userInRole = userInRole;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(ChangeSettingPublicRental command)
        {
            var organization = _organizationRepository.GetById(command.OrganizationId);
            var initiator = _userInRole.GetById(command.InitiatorId);

            _authorize.Enforce(initiator.InitiatorId, Manage.Organization(organization.Id));
            

            organization.ChangeSettingPublicRental(initiator, command.Value);
        }
    }
}