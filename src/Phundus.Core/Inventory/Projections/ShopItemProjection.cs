namespace Phundus.Inventory.Projections
{
    using System;
    using Articles.Model;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Shop.Projections;

    public class ShopItemProjection : ReadModelBase<ShopItemProjectionRow>, IStoredEventsConsumer
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
            if (domainEvent.ArticleGuid == Guid.Empty)
                return;

            var row = CreateRow();
            row.ArticleGuid = domainEvent.ArticleGuid;
            row.ArticleId = domainEvent.ArticleId;
            row.Name = domainEvent.Name;
            row.OwnerGuid = domainEvent.Owner.OwnerId.Id;
            row.OwnerName = domainEvent.Owner.Name;
            row.OwnerType = (int) domainEvent.Owner.Type;
            row.PublicPrice = domainEvent.PublicPrice;
            row.MemberPrice = domainEvent.MemberPrice;

            SaveOrUpdate(row);
        }

        public void Process(ArticleDeleted domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            Session.Delete(row);
        }
        
        public void Process(ArticleDetailsChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.Name = domainEvent.Name;
            row.Brand = domainEvent.Brand;
            row.Color = domainEvent.Color;
        }

        public void Process(DescriptionChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.Description = domainEvent.Description;
        }

        public void Process(SpecificationChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.Specification = domainEvent.Specification;
        }

        public void Process(PricesChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid);
            row.PublicPrice = domainEvent.PublicPrice;
            row.MemberPrice = domainEvent.MemberPrice;
        }

        private ShopItemProjectionRow Find(Guid articleGuid)
        {
            return Session.QueryOver<ShopItemProjectionRow>().
                Where(p => p.ArticleGuid == articleGuid).SingleOrDefault();
        }
    }
}