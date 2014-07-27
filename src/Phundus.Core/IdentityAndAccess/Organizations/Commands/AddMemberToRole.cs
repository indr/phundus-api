namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Cqrs;
    using Repositories;
    using Users.Repositories;

    public class AddMemberToRole
    {
        public int OrganizationId { get; set; }
        public int AdministratorId { get; set; }
        public int MemberId { get; set; }
        public int RoleId { get; set; }
    }

    public class AddMemberToRoleHandler : IHandleCommand<AddMemberToRole>
    {
        public IUserRepository UserRepository { get; set; }
        public IOrganizationRepository OrganizationRepository { get; set; }

        public void Handle(AddMemberToRole command)
        {
            var administrator = UserRepository.ById(command.AdministratorId);
            var member = UserRepository.ById(command.MemberId);
            var organization = OrganizationRepository.ById(command.OrganizationId);

            organization.SetMembersRole(administrator, member, command.RoleId);
        }
    }
}