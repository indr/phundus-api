﻿namespace Phundus.Web.Projections
{
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
}