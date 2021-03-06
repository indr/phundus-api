﻿namespace Phundus.IdentityAccess.Model
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Organizations.Model;
    using Organizations;
    using Users;

    public interface IUserInRoleService
    {
        Admin Admin(UserId userId);
        Founder Founder(UserId userId);
        Manager Manager(UserId userId, OrganizationId organizationId);
        Initiator Initiator(InitiatorId initiatorId);
    }

    public class UserInRoleService : IUserInRoleService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserRepository _userRepository;

        public UserInRoleService(IUserRepository userRepository, IMembershipRepository membershipRepository)
        {
            _userRepository = userRepository;
            _membershipRepository = membershipRepository;
        }

        public Initiator Initiator(InitiatorId initiatorId)
        {
            var result = _userRepository.GetById(initiatorId);
            return new Initiator(new InitiatorId(result.UserId), result.EmailAddress, result.FullName);
        }

        public Admin Admin(UserId userId)
        {
            var user = _userRepository.GetById(userId);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return new Admin(user.UserId, user.EmailAddress, user.FullName);
        }

        public Founder Founder(UserId userId)
        {
            var user = _userRepository.GetById(userId);

            if (user.IsLocked)
                throw new AuthorizationException(String.Format("User {0} is locked.", userId));

            return new Founder(user.UserId, user.EmailAddress, user.FullName);
        }

        public Manager Manager(UserId userId, OrganizationId organizationId)
        {
            var user = _userRepository.GetById(userId);
            if (user.Role == UserRole.Admin)
                return new Manager(user.UserId, user.EmailAddress, user.FullName);

            if (user.UserId.Id == organizationId.Id)
                return new Manager(user.UserId, user.EmailAddress, user.FullName);

            var membership = _membershipRepository.FindByUserId(userId.Id)
                .Where(p => p.MemberRole == MemberRole.Manager)
                .FirstOrDefault(p => Equals(p.OrganizationId, organizationId));

            if (membership == null)
                throw new AuthorizationException(String.Format("Manager {0} for organization {1} not found.", userId,
                    organizationId));

            if (membership.IsLocked)
                throw new AuthorizationException(String.Format("Manager {0} for organization {1} not found.", userId,
                    organizationId));

            return new Manager(user.UserId, user.EmailAddress, user.FullName);
        }
    }
}