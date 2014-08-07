namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;
    using Cqrs;
    using Ddd;
    using Organizations.Model;

    public class RelationshipsReadModel : ReadModelBase, IRelationshipQueries, ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>, ISubscribeTo<MembershipApplicationRejected>
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


            return ToRelationshipDto(organizationId, memberId, membership, application);
        }

        //public RelationshipDto ByMemberIdForOrganizationId(int memberId, int organizationId)
        //{
        //    var result = (from r in Data.RelationshipDtos
        //        where r.OrganizationId == organizationId && r.UserId == memberId
        //        select r).FirstOrDefault();

        //    if (result != null)
        //        return result;

        //    return new RelationshipDto
        //    {
        //        OrganizationId = organizationId,
        //        UserId = memberId,
        //        Status = RelationshipStatusDto.None,
        //        Timestamp = DateTime.UtcNow
        //    };
        //}

        public void Handle(MembershipApplicationApproved @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOn, RelationshipStatusDto.Application);
        }

        public void Handle(MembershipApplicationFiled @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOn, RelationshipStatusDto.Application);
        }

        public void Handle(MembershipApplicationRejected @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOn, RelationshipStatusDto.Application);
        }

        private static RelationshipDto ToRelationshipDto(int organizationId, int userId, MembershipDto membership,
            MembershipApplicationDto application)
        {
            var status = RelationshipStatusDto.None;
            var dateTime = DateTime.Now;

            if (membership != null)
            {
                status = RelationshipStatusDto.Member;
                dateTime = membership.ApprovedOn;
            }

            if ((application != null) && (application.RejectedOn.HasValue))
            {
                status = RelationshipStatusDto.Rejected;
                dateTime = application.RejectedOn.Value;
            }

            if (application != null)
            {
                status = RelationshipStatusDto.Application;
                dateTime = application.CreatedOn;
            }

            return new RelationshipDto
            {
                OrganizationId = organizationId,
                Status = status,
                Timestamp = dateTime.ToUniversalTime(),
                UserId = userId
            };
        }

        private void UpdateOrInsert(int userId, int organizationId, DateTime timestamp, RelationshipStatusDto status)
        {
            return;

            var ctx = new ReadModelDataContext(Session.Connection);
            var entity = (from r in ctx.RelationshipDtos
                where r.OrganizationId == organizationId && r.UserId == userId
                select r).SingleOrDefault();
            if (entity == null)
            {
                entity = new RelationshipDto();
                entity.OrganizationId = organizationId;
                entity.UserId = userId;
                ctx.RelationshipDtos.InsertOnSubmit(entity);
            }

            entity.Timestamp = timestamp;
            entity.Status = status;

            ctx.SubmitChanges();
        }
    }

    public enum RelationshipStatusDto
    {
        None,
        Member,
        Rejected,
        Application
    }
}