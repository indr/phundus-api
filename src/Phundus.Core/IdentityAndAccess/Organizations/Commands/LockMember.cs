namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System.Security;
    using Common.Cqrs;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class LockMember : ICommand
    {
        public int OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int MemberId { get; set; }
    }

    public class LockMemberHandler : IHandleCommand<LockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(LockMember command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var member = UserRepository.GetById(command.MemberId);

            if (member.Id == command.InitiatorId)
                throw new AttemptToLockOneselfException();

            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            organization.LockMember(member);
        }
    }
}