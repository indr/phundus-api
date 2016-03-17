﻿namespace Phundus.IdentityAccess.Resources
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model.Organizations;
    using Organizations.Model;

    public interface IMemberInRoleResource
    {
        MemberInRoleCto Manager(OrganizationId organizationId, UserId userId);
        MemberInRoleCto Member(OrganizationId organizationId, UserId userId);
    }

    public class MemberInRoleCto
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
    }

    public class MemberInRoleResource : IMemberInRoleResource
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUsersQueries _usersQueries;

        public MemberInRoleResource(IUsersQueries usersQueries, IMembershipRepository membershipRepository)
        {
            _usersQueries = usersQueries;
            _membershipRepository = membershipRepository;
        }

        public MemberInRoleCto Manager(OrganizationId organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _usersQueries.FindActiveById(organizationId.Id);
            if ((user != null) && (user.UserId == userId.Id))
                return ToMember(user);

            var membership = _membershipRepository.FindByUserId(userId.Id)
                .Where(p => p.MemberRole == MemberRole.Manager)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId.Id);

            if (membership == null)
                return null;

            if (membership.IsLocked)
                return null;

            return ToMember(user);
        }

        public MemberInRoleCto Member(OrganizationId organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _usersQueries.FindActiveById(userId.Id);
            if ((user != null) && (user.UserId == organizationId.Id))
                return ToMember(user);

            var membership = _membershipRepository.FindByUserId(userId.Id)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId.Id);

            if (membership == null)
                return null;

            if (membership.IsLocked)
                return null;

            return ToMember(user);
        }

        private MemberInRoleCto ToMember(IUser user)
        {
            return new MemberInRoleCto
            {
                UserId = user.UserId,
                EmailAddress = user.EmailAddress,
                FullName = user.FullName
            };
        }
    }
}