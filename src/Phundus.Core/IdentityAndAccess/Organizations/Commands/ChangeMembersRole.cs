namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Cqrs;
    using Model;
    using Repositories;
    using Users.Repositories;

    public class ChangeMembersRole
    {
        public int OrganizationId { get; set; }
        public int AdministratorId { get; set; }
        public int MemberId { get; set; }
        public int Role { get; set; }
    }

    public class ChangeMembersRoleHandler : IHandleCommand<ChangeMembersRole>
    {
        public IUserRepository UserRepository { get; set; }
        public IOrganizationRepository OrganizationRepository { get; set; }

        public void Handle(ChangeMembersRole command)
        {
            var administrator = UserRepository.ById(command.AdministratorId);
            var member = UserRepository.ById(command.MemberId);
            var organization = OrganizationRepository.ById(command.OrganizationId);

            organization.SetMembersRole(administrator, member, (Role)command.Role);
        }
    }
}