﻿namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Model;
    using Queries;
    using Repositories;
    using Users.Repositories;

    public class ChangeMembersRole
    {
        public Guid OrganizationId { get; set; }
        public CurrentUserId InitiatorId { get; set; }
        public UserId MemberId { get; set; }
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

            var member = UserRepository.GetByGuid(command.MemberId);

            MemberInRole.ActiveManager(command.OrganizationId, command.InitiatorId);

            organization.SetMembersRole(member, (MemberRole) command.Role);
        }
    }
}