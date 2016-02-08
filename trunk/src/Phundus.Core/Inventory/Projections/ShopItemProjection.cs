namespace Phundus.Inventory.Projections
{
    using System;
    using Articles.Model;
    using Common;
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
            row.LessorId = domainEvent.Owner.OwnerId.Id;
            row.LessorName = domainEvent.Owner.Name;
            row.LessorType = (int) domainEvent.Owner.Type;
            row.PublicPrice = domainEvent.PublicPrice;
            row.MemberPrice = domainEvent.MemberPrice;

            Insert(row);
        }

        public void Process(ArticleDeleted domainEvent)
        {
            var row = GetRow(domainEvent.ArticleGuid);
            Session.Delete(row);
        }
        
        public void Process(ArticleDetailsChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleGuid);
            row.Name = domainEvent.Name;
            row.Brand = domainEvent.Brand;
            row.Color = domainEvent.Color;
        }

        public void Process(DescriptionChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleGuid);
            row.Description = domainEvent.Description;
        }

        public void Process(SpecificationChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleGuid);
            row.Specification = domainEvent.Specification;
        }

        public void Process(PricesChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleGuid);            
            row.PublicPrice = domainEvent.PublicPrice;
            row.MemberPrice = domainEvent.MemberPrice;
        }

        private ShopItemProjectionRow GetRow(Guid articleGuid)
        {
            var result = Session.QueryOver<ShopItemProjectionRow>().
                Where(p => p.ArticleGuid == articleGuid).SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Shop item projection row {0} not found.", articleGuid);
            return result;
        }
    }
}