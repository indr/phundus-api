namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;
    using Common;
    using Cqrs;
    using Organizations.Model;
    using Organizations.Repositories;

    public class MemberInRoleReadModel : ReadModelBase, IMemberInRole
    {
        public IMembershipRepository MembershipRepository { get; set; }

        public void ActiveMember(Guid organizationId, int userId)
        {
            if (!IsActiveMember(organizationId, userId))
                throw new AuthorizationException("Sie müssen aktives Mitglied dieser Organisation sein.");
        }

        public void ActiveChief(Guid organizationId, int userId)
        {
            if (!IsActiveChief(organizationId, userId))
                throw new AuthorizationException(
                    "Sie müssen aktives Mitglied mit der Rolle Verwaltung dieser Organisation sein.");
        }

        public bool IsActiveMember(Guid organizationId, int userId)
        {
            var membership =
                MembershipRepository.ByMemberId(userId).FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(Guid organizationId, int userId)
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
}