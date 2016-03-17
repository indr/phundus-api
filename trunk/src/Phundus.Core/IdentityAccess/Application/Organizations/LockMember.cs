namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model;
    using Model.Organizations;
    using Users.Services;

    public class LockMember : ICommand
    {
        public LockMember(InitiatorId initiatorId, OrganizationId organizationId, UserId memberId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (memberId == null) throw new ArgumentNullException("memberId");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            MemberId = memberId;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public UserId MemberId { get; protected set; }
    }

    public class LockMemberHandler : IHandleCommand<LockMember>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserInRoleService _userInRoleService;

        public LockMemberHandler(IUserInRoleService userInRoleService, IOrganizationRepository organizationRepository)
        {
            if (userInRoleService == null) throw new ArgumentNullException("userInRoleService");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _userInRoleService = userInRoleService;
            _organizationRepository = organizationRepository;
        }

        [Transaction]
        public void Handle(LockMember command)
        {
            var manager = _userInRoleService.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.LockMember(manager, command.MemberId);
        }
    }
}