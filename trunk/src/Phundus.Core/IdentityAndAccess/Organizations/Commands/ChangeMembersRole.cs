﻿namespace Phundus.Core.IdentityAndAccess.Organizations.Commands
{
    using Cqrs;
    using Model;
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

        public void Handle(ChangeMembersRole command)
        {
            var chief = UserRepository.ById(command.ChiefId);
            var member = UserRepository.ById(command.MemberId);
            var organization = OrganizationRepository.ById(command.OrganizationId);

            organization.SetMembersRole(chief, member, (Role) command.Role);
        }
    }
}