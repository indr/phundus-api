namespace Phundus.IdentityAccess.Queries.EventSourcedViewsUpdater
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Organizations.Model;

    public class EsRelationshipsUpdater : NHibernateReadModelBase<RelationshipViewRow>, IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(MembershipApplicationFiled domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "application");
        }

        public void Process(MembershipApplicationApproved domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "member");
        }

        public void Process(MembershipApplicationRejected domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc, "rejected");
        }

        private void UpdateOrInsert(Guid userId, Guid organizationId, DateTime timestamp, string status)
        {
            var row = Session.QueryOver<RelationshipViewRow>().Where(p =>
                p.UserGuid == userId && p.OrganizationGuid == organizationId).SingleOrDefault();

            if (row == null)
            {
                row = new RelationshipViewRow
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
}