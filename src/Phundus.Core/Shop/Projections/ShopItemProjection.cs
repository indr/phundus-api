namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;

    public class ShopItemProjection : ReadModelBase<ShopItemProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(ArticleCreated domainEvent)
        {
            if (domainEvent.ArticleId == Guid.Empty)
                return;

            var row = CreateRow();
            row.ArticleGuid = domainEvent.ArticleId;
            row.ArticleId = domainEvent.ArticleShortId;
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
            var row = GetRow(domainEvent.ArticleId);
            Session.Delete(row);
        }
        
        public void Process(ArticleDetailsChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleId);
            row.Name = domainEvent.Name;
            row.Brand = domainEvent.Brand;
            row.Color = domainEvent.Color;
        }

        public void Process(DescriptionChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleId);
            row.Description = domainEvent.Description;
        }

        public void Process(SpecificationChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleId);
            row.Specification = domainEvent.Specification;
        }

        public void Process(PricesChanged domainEvent)
        {
            var row = GetRow(domainEvent.ArticleId);            
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

    public class ShopItemProjectionRow
    {
        public virtual Guid ArticleGuid { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Color { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid LessorId { get; set; }
        public virtual string LessorName { get; set; }
        public virtual int LessorType { get; set; }
        public virtual string Description { get; set; }
        public virtual string Specification { get; set; }

        public virtual ICollection<ShopItemFilesProjectionRow> Documents { get; set; }
        public virtual ICollection<ShopItemImagesProjectionRow> Images { get; set; }
    }
}