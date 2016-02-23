namespace Phundus.IdentityAccess.Users.Services
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Model;
    using Organizations.Model;
    using Organizations.Repositories;
    using Repositories;

    public interface IUserInRole
    {
        Admin Admin(UserId userId);
        Founder Founder(UserId userId);
        Manager Manager(UserId userId, OrganizationId organizationId);

        bool IsAdmin(UserId userId);
    }

    public class UserInRole : IUserInRole
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserRepository _userRepository;

        public UserInRole(IUserRepository userRepository, IMembershipRepository membershipRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            if (membershipRepository == null) throw new ArgumentNullException("membershipRepository");

            _userRepository = userRepository;
            _membershipRepository = membershipRepository;
        }

        public Admin Admin(UserId userId)
        {
            var user = _userRepository.GetByGuid(userId);
            if (user.Role != UserRole.Admin)
                throw new AuthorizationException("Sie müssen Administratorenrechte haben.");

            return new Admin(user.UserId, user.EmailAddress, user.FullName);
        }

        public Founder Founder(UserId userId)
        {
            var user = _userRepository.GetByGuid(userId);

            if (user.IsLocked)
                throw new AuthorizationException(String.Format("User {0} is locked.", userId));

            return new Founder(user.UserId, user.EmailAddress, user.FullName);
        }

        public Manager Manager(UserId userId, OrganizationId organizationId)
        {
            var user = _userRepository.GetByGuid(userId);
            if (user.Role == UserRole.Admin)
                return new Manager(user.UserId, user.EmailAddress, user.FullName);

            var membership = _membershipRepository.ByMemberId(userId.Id)
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

        public bool IsAdmin(UserId userId)
        {
            var user = _userRepository.GetByGuid(userId);
            return user.Role == UserRole.Admin;
        }
    }
}