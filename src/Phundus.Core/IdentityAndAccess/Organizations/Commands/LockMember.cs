namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System.Security;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class LockMember
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
            var organization = OrganizationRepository.ById(command.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException();

            var member = UserRepository.ById(command.MemberId);
            if (member == null)
                throw new MemberNotFoundException();

            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            organization.LockMember(member);
        }
    }
}