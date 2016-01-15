namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Exceptions;
    using IdentityAccess.Users.Repositories;
    using Queries;
    using Repositories;

    public class LockMember
    {
        public Guid OrganizationId { get; set; }
        public CurrentUserGuid InitiatorId { get; set; }
        public UserGuid MemberId { get; set; }
    }

    public class LockMemberHandler : IHandleCommand<LockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(LockMember command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var member = UserRepository.GetByGuid(command.MemberId);

            if (Equals(member.UserGuid, command.InitiatorId))
                throw new AttemptToLockOneselfException();

            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            organization.LockMember(member);
        }
    }
}