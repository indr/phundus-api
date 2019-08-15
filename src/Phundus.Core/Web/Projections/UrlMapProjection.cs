namespace Phundus.Web.Projections
{
    using Application;
    using Common;
    using Common.Eventing;
    using Common.Projecting;
    using IdentityAccess.Organizations.Model;

    public class UrlMapProjection : ProjectionBase<UrlMapData>,
        ISubscribeTo<OrganizationEstablished>,
        ISubscribeTo<OrganizationRenamed>
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

        public void Handle(OrganizationRenamed e)
        {
            var url = e.Name.ToFriendlyUrl();

            Update(x => x.OrganizationId == e.OrganizationId, x => { x.Url = url; });
        }
    }
}