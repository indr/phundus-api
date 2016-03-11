namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common.Eventing;
    using Common.Projecting;
    using Organizations.Model;

    public class RelationshipProjection : ProjectionBase<RelationshipData>,
        ISubscribeTo<MembershipApplicationApproved>,
        ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationRejected>
    {
        public void Handle(MembershipApplicationApproved domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "member");
        }

        public void Handle(MembershipApplicationFiled domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "application");
        }

        public void Handle(MembershipApplicationRejected domainEvent)
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