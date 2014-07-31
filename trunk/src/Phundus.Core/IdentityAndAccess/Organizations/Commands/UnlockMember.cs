namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System.Security;
    using Cqrs;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class UnlockMember
    {
        public int OrganizationId { get; set; }
        public int ChiefId { get; set; }
        public int MemberId { get; set; }
    }

    public class UnlockMemberHandler : IHandleCommand<UnlockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMemberInMembershipRoleQueries MemberInMembershipRoleQueries { get; set; }

        public void Handle(UnlockMember command)
        {
            var organization = OrganizationRepository.ById(command.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException();

            var member = UserRepository.ById(command.MemberId);
            if (member == null)
                throw new MemberNotFoundException();

            if (!MemberInMembershipRoleQueries.IsActiveChiefIn(command.OrganizationId, command.ChiefId))
                throw new SecurityException();

            organization.UnlockMember(member);
        }
    }
}