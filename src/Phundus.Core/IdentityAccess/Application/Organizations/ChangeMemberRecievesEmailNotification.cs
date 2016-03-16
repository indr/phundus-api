namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Organizations;
    using Users.Services;

    public class ChangeMemberRecievesEmailNotification : ICommand
    {
        public ChangeMemberRecievesEmailNotification(InitiatorId initiatorId, OrganizationId organizationId, UserId memberId, bool value)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            MemberId = memberId;
            Value = value;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public UserId MemberId { get; protected set; }
        public bool Value { get; protected set; }
    }

    public class ChangeMemberRecievesEmailNotificationHandler : IHandleCommand<ChangeMemberRecievesEmailNotification>
    {
        private readonly IUserInRole _userInRole;
        private readonly IOrganizationRepository _organizationRepository;

        public ChangeMemberRecievesEmailNotificationHandler(IUserInRole userInRole, IOrganizationRepository organizationRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _userInRole = userInRole;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(ChangeMemberRecievesEmailNotification command)
        {
            var manager = _userInRole.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.ChangeMembersRecieveEmailNotificationOption(manager, command.MemberId, command.Value);
        }
    }
}