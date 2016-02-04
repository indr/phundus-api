namespace Phundus.Shop.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;
    using NHibernate;

    public class ResultItemsProjection : ReadModelBase<ResultItemsProjectionRow>, IDomainEventHandler
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(ArticleCreated domainEvent)
        {
            var row = CreateRow();
            row.ArticleGuid = domainEvent.ArticleGuid;
            row.ArticleId = domainEvent.ArticleId;
            row.MemberPrice = domainEvent.MemberPrice;
            row.Name = domainEvent.Name;
            row.OwnerGuid = domainEvent.Owner.OwnerId.Id;
            row.OwnerName = domainEvent.Owner.Name;
            row.OwnerType = (int)domainEvent.Owner.Type;
            row.PreviewImageFileName = "";
            row.PublicPrice = domainEvent.PublicPrice;
        }

        public void Process(ArticleDetailsChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.Name = domainEvent.Name;
        }

        public void Process(ArticleDeleted domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            Delete(row);
        }

        public void Process(PreviewImageChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.PreviewImageFileName = domainEvent.FileName;            
        }

        public void Process(PricesChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.MemberPrice = domainEvent.MemberPrice;
            row.PublicPrice = domainEvent.PublicPrice;
        }

        private ResultItemsProjectionRow Find(Guid articleGuid)
        {
            return Session.QueryOver<ResultItemsProjectionRow>()
                .Where(p => p.ArticleGuid == articleGuid).SingleOrDefault();
        }
    }
}