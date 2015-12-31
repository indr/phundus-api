namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Organizations.Model;
    using Organizations.Repositories;

    public class MemberInRoleReadModel : ReadModelBase, IMemberInRole
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserQueries _userQueries;

        public MemberInRoleReadModel(IUserQueries userQueries, IMembershipRepository membershipRepository)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(membershipRepository, "MembershipRepository must be provided.");

            _userQueries = userQueries;
            _membershipRepository = membershipRepository;
        }

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

        public void ActiveChief(OwnerId ownerId, int userId)
        {
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");

            ActiveChief(ownerId.Id, userId);
        }

        public void ActiveChief(OwnerId ownerId, UserId userId)
        {
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotNull(userId, "UserId must be provided.");

            ActiveChief(ownerId.Id, userId.Id);
        }

        public bool IsActiveMember(Guid organizationId, int userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _userQueries.FindActiveById(organizationId);
            if (user != null)
                return true;

            var membership = _membershipRepository.ByMemberId(userId)
                .FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(Guid organizationId, int userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _userQueries.FindActiveById(organizationId);
            if ((user != null) && (user.Id == userId))
                return true;

            var membership = _membershipRepository.ByMemberId(userId)
                .Where(p => p.Role == Role.Chief)
                .FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(OwnerId ownerId, int userId)
        {
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");

            return IsActiveChief(ownerId.Id, userId);
        }

        public void IsActiveChief(OwnerId ownerId, UserId userId)
        {
            AssertionConcern.AssertArgumentNotNull(ownerId, "OwnerId must be provided.");
            AssertionConcern.AssertArgumentNotNull(userId, "UserId must be provided.");

            IsActiveChief(ownerId.Id, userId.Id);
        }
    }
}