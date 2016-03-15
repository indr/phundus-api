namespace Phundus.IdentityAccess.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Model.Organizations;
    using Model.Users;
    using Organizations.Model;
    using Projections;
    using Resources;

    public class ChangeMembersRole : ICommand
    {
        public ChangeMembersRole(InitiatorId initiatorId, OrganizationId organizationId, UserId memberId,
            MemberRole memberRole)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (memberId == null) throw new ArgumentNullException("memberId");
            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            MemberId = memberId;
            MemberRole = memberRole;
        }

        public InitiatorId InitiatorId { get; protected set; }
        public OrganizationId OrganizationId { get; protected set; }
        public UserId MemberId { get; protected set; }
        public MemberRole MemberRole { get; protected set; }        
    }

    public class ChangeMembersRoleHandler : IHandleCommand<ChangeMembersRole>
    {
        public IUserRepository UserRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        [Transaction]
        public void Handle(ChangeMembersRole command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var member = UserRepository.GetById(command.MemberId);

            MemberInRole.ActiveManager(command.OrganizationId, command.InitiatorId);

            organization.SetMembersRole(member, command.MemberRole);
        }
    }
}