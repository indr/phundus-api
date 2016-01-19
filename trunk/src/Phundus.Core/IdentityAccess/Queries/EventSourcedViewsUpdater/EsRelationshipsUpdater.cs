namespace Phundus.IdentityAccess.Queries.EventSourcedViewsUpdater
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Organizations.Model;
    using ReadModels;

    public class RelationshipViewRow
    {
        private Guid _relationshipGuid = Guid.NewGuid();

        public virtual Guid RelationshipGuid
        {
            get { return _relationshipGuid; }
            set { _relationshipGuid = value; }
        }

        public virtual Guid OrganizationGuid { get; set; }

        public virtual Guid UserGuid { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual string Status { get; set; }
    }

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
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc,
                RelationshipStatusDto.Application);
        }

        public void Process(MembershipApplicationApproved domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc,
                RelationshipStatusDto.Member);
        }

        public void Process(MembershipApplicationRejected domainEvent)
        {
            UpdateOrInsert(domainEvent.UserGuid, domainEvent.OrganizationGuid, domainEvent.OccuredOnUtc,
                RelationshipStatusDto.Rejected);
        }

        private void UpdateOrInsert(Guid userId, Guid organizationId, DateTime timestamp, RelationshipStatusDto status)
        {
            var row = QueryOver().Where(p => 
                    p.UserGuid == userId && p.OrganizationGuid == organizationId).SingleOrDefault();
            if (row == null)
            {
                row = new RelationshipViewRow
                {
                    OrganizationGuid = organizationId,
                    UserGuid = userId,
                    Timestamp = timestamp,
                    Status = status.ToString()
                };
                Session.Save(row);
            }
            else
            {
                row.Status = status.ToString();
                row.Timestamp = timestamp;

                Session.SaveOrUpdate(row);
            }
        }
    }
}