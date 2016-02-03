namespace Phundus.IdentityAccess.Projections
{
    using System;

    public class RelationshipProjectionRow
    {
        public virtual Guid RowGuid { get; set; }
        public virtual Guid OrganizationGuid { get; set; }
        public virtual Guid UserGuid { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string Status { get; set; }
    }
}