namespace Phundus.Web.Projections
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using IdentityAccess.Organizations.Model;

    public class FriendlyUrlProjection : ProjectionBase<FriendlyUrlProjectionRow>, IStoredEventsConsumer,
        IFriendlyUrlQueries
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        private void Process(OrganizationEstablished domainEvent)
        {
            var friendlyUrl = domainEvent.Name.ToFriendlyUrl();

            var row = FindByUrl(friendlyUrl);
            if (row == null)
                row = CreateRow();
            row.Url = friendlyUrl;
            row.OrganizationId = domainEvent.OrganizationId;

            SaveOrUpdate(row);
        }

        public FriendlyUrlProjectionRow FindByUrl(string url)
        {
            return QueryOver().Where(p => p.Url == url).SingleOrDefault();
        }
    }

    public interface IFriendlyUrlQueries
    {
        FriendlyUrlProjectionRow FindByUrl(string url);
    }

    public class FriendlyUrlProjectionRow
    {
        public virtual Guid RowId { get; set; }
        public virtual string Url { get; set; }
        public virtual Guid? OrganizationId { get; set; }
        public virtual Guid? UserId { get; set; }
    }
}