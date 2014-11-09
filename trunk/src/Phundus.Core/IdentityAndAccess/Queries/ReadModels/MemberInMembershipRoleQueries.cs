namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;
    using Cqrs;
    using Domain.Model.Organizations;
    using Domain.Model.Users;
    using Organizations.Repositories;
    using Users.Model;
    using Role = Organizations.Model.Role;

    public class MemberInRoleReadModel : ReadModelBase, IMemberInRole
    {
        public IMembershipRepository MembershipRepository { get; set; }

        public void ActiveMember(int organizationId, int userId)
        {
            if (!IsActiveMember(organizationId, userId))
                throw new AuthorizationException("Sie müssen aktives Mitglied dieser Organisation sein.");
        }

        public void ActiveChief(OrganizationId organizationId, UserId userId)
        {
            ActiveChief(organizationId.Id, userId.Id);
        }

        public void ActiveMember(OrganizationId organizationId, UserId userId)
        {
            ActiveMember(organizationId.Id, userId.Id);
        }

        public void ActiveChief(int organizationId, int userId)
        {
            if (!IsActiveChief(organizationId, userId))
                throw new AuthorizationException(
                    "Sie müssen aktives Mitglied mit der Rolle Verwaltung dieser Organisation sein.");
        }

        public bool IsActiveMember(int organizationId, int userId)
        {
            var membership =
                MembershipRepository.ByMemberId(userId).FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(int organizationId, int userId)
        {
            var membership = MembershipRepository.ByMemberId(userId)
                .Where(p => p.Role == Role.Chief)
                .FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }
    }

    public class AuthorizationException : Exception
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string message) : base(message)
        {
        }
    }
}