namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Linq;

    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(int memberId, int organizationId);
    }

    public class RelationshipsReadModel : IRelationshipQueries
    {
        public IMembershipQueries MembershipQueries { get; set; }

        public IMembershipApplicationQueries MembershipApplicationQueries { get; set; }

        public RelationshipDto ByMemberIdForOrganizationId(int memberId, int organizationId)
        {
            var membership = MembershipQueries.ByMemberId(memberId)
                .FirstOrDefault(p => p.OrganizationId == organizationId);

            var application =
                MembershipApplicationQueries.PendingByOrganizationId(organizationId)
                    .FirstOrDefault(p => p.UserId == memberId);


            return ToRelationshipDto(membership, application);
        }

        private static RelationshipDto ToRelationshipDto(MembershipDto membership, MembershipApplicationDto application)
        {
            return new RelationshipDto
            {
                Membership = membership,
                Application = application
            };
        }
    }

    public class RelationshipDto
    {
        public MembershipDto Membership { get; set; }
        public MembershipApplicationDto Application { get; set; }
    }
}