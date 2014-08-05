namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;

    public class RelationshipsReadModel : IRelationshipQueries
    {
        public IMembershipQueries MembershipQueries { get; set; }

        public IMembershipApplicationQueries MembershipApplicationQueries { get; set; }

        public RelationshipDto ByMemberIdForOrganizationId(int memberId, int organizationId)
        {
            var membership = MembershipQueries.ByUserId(memberId)
                .FirstOrDefault(p => p.OrganizationId == organizationId);

            var application =
                MembershipApplicationQueries.PendingByOrganizationId(organizationId)
                    .FirstOrDefault(p => p.UserId == memberId);


            return ToRelationshipDto(membership, application);
        }

        private static RelationshipDto ToRelationshipDto(MembershipDto membership, MembershipApplicationDto application)
        {
            var dateTime = DateTime.Now;
            if (membership != null)
                return new RelationshipDto(RelationshipDto.StatusDto.Member, membership.ApprovedOn);

            if ((application != null) && (application.RejectedOn.HasValue))
                return new RelationshipDto(RelationshipDto.StatusDto.Rejected, application.RejectedOn.Value);

            if (application != null)
                return new RelationshipDto(RelationshipDto.StatusDto.Application, application.CreatedOn);

            return new RelationshipDto(RelationshipDto.StatusDto.None, null);
        }
    }

    public class RelationshipDto
    {
        public enum StatusDto
        {
            None, Member, Rejected, Application
        }

        public RelationshipDto(StatusDto status, DateTime? dateTime)
        {
            Status = status;
            DateTime = dateTime;
        }

        public StatusDto Status { get; set; }
        public string StatusString { get { return Status.ToString(); } }
        public DateTime? DateTime { get; set; }
    }
}