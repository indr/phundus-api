namespace Phundus.Web.Projections
{
    using System;
    using Common;
    using Common.Eventing;
    using Common.Projecting;
    using IdentityAccess.Organizations.Model;

    public class UrlMapProjection : ProjectionBase<UrlMapData>,
        ISubscribeTo<OrganizationEstablished>
    {
        public void Handle(OrganizationEstablished e)
        {
            var url = e.Name.ToFriendlyUrl();

            InsertOrUpdate(p => p.Url == url, data =>
            {
                data.Url = url;
                data.OrganizationId = e.OrganizationId;
            });
        }
    }

    public class UrlMapData
    {
        public virtual Guid RowId { get; set; }
        public virtual string Url { get; set; }
        public virtual Guid? OrganizationId { get; set; }
        public virtual Guid? UserId { get; set; }
    }
}