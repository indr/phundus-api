namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
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

        public void ActiveMember(Guid organizationId, UserId userId)
        {
            if (!IsActiveMember(organizationId, userId))
                throw new AuthorizationException("Sie müssen aktives Mitglied dieser Organisation sein.");
        }

        public void ActiveMember(OwnerId ownerId, UserId userId)
        {
            ActiveMember(ownerId.Id, userId);
        }

        public void ActiveChief(Guid organizationId, UserId userId)
        {
            if (!IsActiveChief(organizationId, userId))
                throw new AuthorizationException(
                    "Sie müssen aktives Mitglied mit der Rolle Verwaltung dieser Organisation sein.");
        }

        public void ActiveChief(OwnerId ownerId, UserId userId)
        {
            ActiveChief(ownerId.Id, userId);
        }

        public bool IsActiveMember(Guid organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _userQueries.FindActiveById(organizationId);
            if ((user != null) && (user.UserId == userId.Id))
                return true;

            var membership = _membershipRepository.ByMemberId(userId.Id)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveMember(OwnerId ownerId, UserId userId)
        {
            return IsActiveMember(ownerId.Id, userId);
        }

        public bool IsActiveChief(Guid organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _userQueries.FindActiveById(organizationId);
            if ((user != null) && (user.UserId == userId.Id))
                return true;

            var membership = _membershipRepository.ByMemberId(userId.Id)
                .Where(p => p.Role == Role.Chief)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(OwnerId ownerId, UserId userId)
        {
            return IsActiveChief(ownerId.Id, userId);
        }
    }
}