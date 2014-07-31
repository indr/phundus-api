namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Linq;
    using Cqrs;
    using Organizations.Model;
    using Organizations.Repositories;

    public class MemberInMembershipRoleReadModel : ReadModelBase, IMemberInMembershipRoleQueries
    {
        public IMembershipRepository MembershipRepository { get; set; }

        public bool IsActiveMemberIn(int organizationId, int userId)
        {
            var membership = MembershipRepository.ByMemberId(userId).FirstOrDefault(p => p.Organization.Id == organizationId);
            
            if (membership == null)
                return false;

            if (membership.IsLocked)
                return false;

            return true;
        }

        public bool IsActiveChiefIn(int organizationId, int userId)
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