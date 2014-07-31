namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System.Security;
    using Cqrs;
    using Model;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class ChangeMembersRole
    {
        public int OrganizationId { get; set; }
        public int ChiefId { get; set; }
        public int MemberId { get; set; }
        public int Role { get; set; }
    }

    public class ChangeMembersRoleHandler : IHandleCommand<ChangeMembersRole>
    {
        public IUserRepository UserRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInMembershipRoleQueries MemberInMembershipRoleQueries { get; set; }

        public void Handle(ChangeMembersRole command)
        {
            var organization = OrganizationRepository.ById(command.OrganizationId);
            if (organization == null)
                throw new OrganizationNotFoundException();

            var member = UserRepository.ById(command.MemberId);
            if (member == null)
                throw new MemberNotFoundException();
            
            if (!MemberInMembershipRoleQueries.IsActiveChiefIn(command.OrganizationId, command.ChiefId))
                throw new SecurityException();

            organization.SetMembersRole(member, (Role) command.Role);
        }
    }
}