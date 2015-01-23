namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System.Security;
    using Common.Cqrs;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class UnlockMember : ICommand
    {
        public int OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int MemberId { get; set; }
    }

    public class UnlockMemberHandler : IHandleCommand<UnlockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UnlockMember command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var member = UserRepository.GetById(command.MemberId);

            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            organization.UnlockMember(member);
        }
    }
}