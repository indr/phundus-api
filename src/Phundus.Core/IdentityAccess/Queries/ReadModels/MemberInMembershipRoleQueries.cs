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

        public void ActiveMember(Guid organizationId, UserGuid userGuid)
        {
            if (!IsActiveMember(organizationId, userGuid))
                throw new AuthorizationException("Sie müssen aktives Mitglied dieser Organisation sein.");
        }

        public void ActiveMember(OwnerId ownerId, UserGuid userGuid)
        {
            ActiveMember(ownerId.Id, userGuid);
        }

        public void ActiveChief(Guid organizationId, UserGuid userGuid)
        {
            if (!IsActiveChief(organizationId, userGuid))
                throw new AuthorizationException(
                    "Sie müssen aktives Mitglied mit der Rolle Verwaltung dieser Organisation sein.");
        }

        public void ActiveChief(OwnerId ownerId, UserGuid userGuid)
        {
            ActiveChief(ownerId.Id, userGuid);
        }

        public bool IsActiveMember(Guid organizationId, UserGuid userGuid)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _userQueries.FindActiveById(organizationId);
            if ((user != null) && (user.UserGuid == userGuid.Id))
                return true;

            var membership = _membershipRepository.ByMemberId(userGuid.Id)
                .FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveMember(OwnerId ownerId, UserGuid userGuid)
        {
            return IsActiveMember(ownerId.Id, userGuid);
        }

        public bool IsActiveChief(Guid organizationId, UserGuid userGuid)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _userQueries.FindActiveById(organizationId);
            if ((user != null) && (user.UserGuid == userGuid.Id))
                return true;

            var membership = _membershipRepository.ByMemberId(userGuid.Id)
                .Where(p => p.Role == Role.Chief)
                .FirstOrDefault(p => p.Organization.Id == organizationId);

            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(OwnerId ownerId, UserGuid userGuid)
        {
            return IsActiveChief(ownerId.Id, userGuid);
        }
    }
}