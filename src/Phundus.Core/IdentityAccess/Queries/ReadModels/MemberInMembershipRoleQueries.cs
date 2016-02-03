namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Cqrs;
    using Integration.IdentityAccess;
    using NHibernate;
    using Organizations.Model;
    using Organizations.Repositories;

    public class MemberInRoleReadModel : ReadModelBase, IMemberInRole
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserQueries _userQueries;

        public MemberInRoleReadModel(Func<ISession> sessionFactory, IUserQueries userQueries, IMembershipRepository membershipRepository) :base(sessionFactory)
        {
            AssertionConcern.AssertArgumentNotNull(userQueries, "UserQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(membershipRepository, "MembershipRepository must be provided.");

            _userQueries = userQueries;
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

        private bool IsActiveManager(Guid organizationId, UserId userId)
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

        public bool IsActiveMember(LessorId lessorId, UserId userId)
        {
            return IsActiveMember(lessorId.Id, userId);
        }

        public bool IsActiveMember(OrganizationId organizationId, UserId userId)
        {
            return IsActiveMember(organizationId.Id, userId);
        }

        private bool IsActiveMember(Guid organizationId, UserId userId)
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
    }
}