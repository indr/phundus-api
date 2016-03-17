namespace Phundus.Web.Projections
{
    using System;

    public class UrlMapData
    {
        public virtual Guid RowId { get; set; }
        public virtual string Url { get; set; }
        public virtual Guid? OrganizationId { get; set; }
        public virtual Guid? UserId { get; set; }
    }
}