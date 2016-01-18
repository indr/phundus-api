namespace Phundus.IdentityAccess.Organizations.Commands
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using IdentityAccess.Users.Repositories;
    using Queries;
    using Repositories;

    public class UnlockMember
    {
        public Guid OrganizationId { get; set; }
        public CurrentUserId InitiatorId { get; set; }
        public UserGuid MemberId { get; set; }
    }

    public class UnlockMemberHandler : IHandleCommand<UnlockMember>
    {
        public IOrganizationRepository OrganizationRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IMemberInRole MemberInRole { get; set; }

        public void Handle(UnlockMember command)
        {
            var organization = OrganizationRepository.GetById(command.OrganizationId);

            var member = UserRepository.GetByGuid(command.MemberId);

            MemberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            organization.UnlockMember(member);
        }
    }
}