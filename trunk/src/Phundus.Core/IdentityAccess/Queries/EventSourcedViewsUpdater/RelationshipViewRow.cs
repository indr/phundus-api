namespace Phundus.IdentityAccess.Queries.EventSourcedViewsUpdater
{
    using System;

    public class RelationshipViewRow
    {
        public virtual Guid RelationshipGuid { get; set; }

        public virtual Guid OrganizationGuid { get; set; }

        public virtual Guid UserGuid { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual string Status { get; set; }
    }
}