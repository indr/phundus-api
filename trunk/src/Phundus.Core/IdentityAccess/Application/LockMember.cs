namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Commanding;
    using Common.Domain.Model;
    using Organizations.Repositories;
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
        private readonly IUserInRole _userInRole;

        public LockMemberHandler(IUserInRole userInRole, IOrganizationRepository organizationRepository)
        {
            if (userInRole == null) throw new ArgumentNullException("userInRole");
            if (organizationRepository == null) throw new ArgumentNullException("organizationRepository");
            _userInRole = userInRole;
            _organizationRepository = organizationRepository;
        }

        public void Handle(LockMember command)
        {
            var manager = _userInRole.Manager(command.InitiatorId, command.OrganizationId);
            var organization = _organizationRepository.GetById(command.OrganizationId);

            organization.LockMember(manager, command.MemberId);
        }
    }
}