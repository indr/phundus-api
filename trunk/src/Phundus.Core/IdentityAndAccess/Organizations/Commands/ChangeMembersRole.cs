namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using System.Security;
    using Common.Cqrs;
    using Cqrs;
    using Model;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class ChangeMembersRole : ICommand
    {
        public int OrganizationId { get; set; }
        public int InitiatorId { get; set; }
        public int MemberId { get; set; }
        public int Role { get; set; }
    }

    public class ChangeMembersRoleHandler : IHandleCommand<ChangeMembersRole>
    {
        public IUserRepository UserRepository { get; set; }

        public IOrganizationRepository OrganizationRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(ChangeMembersRole command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var member = UserRepository.GetById(command.MemberId);

            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            organization.SetMembersRole(member, (Role) command.Role);
        }
    }
}