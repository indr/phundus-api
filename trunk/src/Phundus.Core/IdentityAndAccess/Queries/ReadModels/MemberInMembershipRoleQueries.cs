namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Linq;
    using System.Security;
    using Cqrs;
    using Organizations.Model;
    using Organizations.Repositories;

    public class MemberInRoleReadModel : ReadModelBase, IMemberInRole
    {
        public IMembershipRepository MembershipRepository { get; set; }

        public void ActiveMember(int organizationId, int userId)
        {
            if (!IsActiveMember(organizationId, userId))
                throw new SecurityException();
        }

        public void ActiveChief(int organizationId, int userId)
        {
            if (!IsActiveChief(organizationId, userId))
                throw new SecurityException();
        }

        public bool IsActiveMember(int organizationId, int userId)
        {
            var membership = MembershipRepository.ByMemberId(userId).FirstOrDefault(p => p.Organization.Id == organizationId);
            
            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChief(int organizationId, int userId)
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