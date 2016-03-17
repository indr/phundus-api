namespace Phundus.IdentityAccess.Resources
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Common.Resources;
    using Model.Organizations;
    using Model.Users;
    using Organizations.Model;
    using Users.Model;

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

    public class MemberInRoleResource : ApiControllerBase, IMemberInRoleResource
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserRepository _usersQueries;

        public MemberInRoleResource(IUserRepository usersQueries, IMembershipRepository membershipRepository)
        {
            _usersQueries = usersQueries;
            _membershipRepository = membershipRepository;
        }

        public MemberInRoleCto Manager(OrganizationId organizationId, UserId userId)
        {
            // Hack für Material-Kontext: organizationId kann die Guid des Benutzers (Owners) sein.
            var user = _usersQueries.FindByGuid(userId.Id);
            if ((user != null) && (user.UserId.Id == organizationId.Id))
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
            var user = _usersQueries.FindByGuid(userId.Id);
            if ((user != null) && (user.UserId.Id == organizationId.Id))
                return ToMember(user);

            var membership = _membershipRepository.FindByUserId(userId.Id)
                .FirstOrDefault(p => p.Organization.Id.Id == organizationId.Id);

            if (membership == null)
                return null;

            if (membership.IsLocked)
                return null;

            return ToMember(user);
        }

        private MemberInRoleCto ToMember(User user)
        {
            if (user == null)
                return null;
            return new MemberInRoleCto
            {
                UserId = user.UserId.Id,
                EmailAddress = user.EmailAddress,
                FullName = user.FullName
            };
        }
    }
}