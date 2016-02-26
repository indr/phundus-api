namespace Phundus.Web.Projections
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAccess.Organizations.Model;
    using Inventory.Stores.Model;

    public interface IUrlMapQueries
    {
        UrlMapData FindByUrl(string url);
    }

    public class UrlMapProjection : ProjectionBase<UrlMapData>, IUrlMapQueries, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        public UrlMapData FindByUrl(string url)
        {
            return SingleOrDefault(p => p.Url == url);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(OrganizationEstablished e)
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