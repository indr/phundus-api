namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Organizations.Model;

    public interface IRelationshipQueries
    {
        RelationshipData ByMemberIdForOrganizationId(UserId memberId, Guid organizationId);
    }

    public class RelationshipProjection : ProjectionBase<RelationshipData>, IRelationshipQueries,
        IStoredEventsConsumer
    {
        public RelationshipData ByMemberIdForOrganizationId(UserId memberId, Guid organizationId)
        {
            var result = QueryOver().Where(p =>
                p.OrganizationGuid == organizationId && p.UserGuid == memberId.Id).SingleOrDefault();

            if (result != null)
                return result;

            return new RelationshipData
            {
                OrganizationGuid = organizationId,
                UserGuid = memberId.Id,
                Status = null,
                Timestamp = DateTime.UtcNow
            };
        }

        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        private void Process(MembershipApplicationFiled domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "application");
        }

        private void Process(MembershipApplicationApproved domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "member");
        }

        private void Process(MembershipApplicationRejected domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "rejected");
        }

        private void UpdateOrInsert(Guid userId, Guid organizationId, DateTime timestamp, string status)
        {
            var row = Session.QueryOver<RelationshipData>().Where(p =>
                p.UserGuid == userId && p.OrganizationGuid == organizationId).SingleOrDefault();

            if (row == null)
            {
                row = new RelationshipData
                {
                    OrganizationGuid = organizationId,
                    UserGuid = userId,
                    Status = status,
                    Timestamp = timestamp
                };
                Session.Save(row);
                Session.Flush();
                return;
            }

            row.Status = status;
            row.Timestamp = timestamp;
            Session.Update(row);
        }
    }

    public class RelationshipData
    {
        public virtual Guid RowGuid { get; set; }
        public virtual Guid OrganizationGuid { get; set; }
        public virtual Guid UserGuid { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string Status { get; set; }
    }
}