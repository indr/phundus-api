namespace Phundus.IdentityAccess.Resources
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Organizations;
    using Organizations.Model;

    public interface IMemberInRole
    {
        void ActiveManager(OrganizationId organizationId, UserId userId);
        void ActiveManager(OwnerId ownerId, UserId userId);
        void ActiveManager(Guid organizationId, UserId userId);

        bool IsActiveManager(OrganizationId organizationId, UserId userId);
        bool IsActiveManager(OwnerId ownerId, UserId userId);

        bool IsActiveMember(LessorId lessorId, UserId userId);
    }

    public class MemberInRoleProjection : IMemberInRole
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUsersQueries _usersQueries;

        public MemberInRoleProjection(IUsersQueries usersQueries, IMembershipRepository membershipRepository)
        {
            _usersQueries = usersQueries;
            _membershipRepository = membershipRepository;
        }

        public void ActiveManager(OrganizationId organizationId, UserId userId)
        {
            ActiveManager(organizationId.Id, userId);
        }

        public void ActiveManager(OwnerId ownerId, UserId userId)
        {
            ActiveManager(ownerId.Id, userId);
        }

        public void ActiveManager(Guid organizationId, UserId userId)
        {
            if (!IsActiveManager(organizationId, userId))
                throw new AuthorizationException(
                    "Sie müssen aktives Mitglied mit der Rolle Verwaltung dieser Organisation sein.");
        }

        public bool IsActiveManager(OrganizationId organizationId, UserId userId)
        {
            return IsActiveManager(organizationId.Id, userId);
        }

        public bool IsActiveManager(OwnerId ownerId, UserId userId)
        {
            return IsActiveManager(ownerId.Id, userId);
        }

        public bool IsActiveMember(LessorId lessorId, UserId userId)
        {
            return IsActiveMember(lessorId.Id, userId);
        }

        private bool IsActiveManager(Guid organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _usersQueries.FindActiveById(organizationId);
            if ((user != null) && (user.UserId == userId.Id))
                return true;

            var membership = _membershipRepository.FindByUserId(userId.Id)
                .Where(p => p.MemberRole == MemberRole.Manager)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        private bool IsActiveMember(Guid organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _usersQueries.FindActiveById(userId.Id);
            if ((user != null) && (user.UserId == organizationId))
                return true;

            var membership = _membershipRepository.FindByUserId(userId.Id)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }
    }
}