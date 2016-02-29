namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;
    using Inventory.Stores.Model;

    public class ShopItemsProjection : ProjectionBase<ShopItemsData>, IStoredEventsConsumer
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
            Insert(x =>
            {
                x.ItemId = domainEvent.ArticleId;
                x.ItemShortId = domainEvent.ArticleShortId;
                x.CreatedAtUtc = domainEvent.OccuredOnUtc;
                x.MemberPrice = domainEvent.MemberPrice;
                x.Name = domainEvent.Name;
                x.OwnerGuid = domainEvent.Owner.OwnerId.Id;
                x.OwnerName = domainEvent.Owner.Name;
                x.OwnerType = (int) domainEvent.Owner.Type;
                x.StoreId = domainEvent.StoreId;
                x.StoreName = domainEvent.StoreName;
                x.PreviewImageFileName = "";
                x.PublicPrice = domainEvent.PublicPrice;
            });
        }

        public void Process(ArticleDeleted e)
        {
            Delete(e.ArticleId);
        }

        public void Process(ArticleDetailsChanged e)
        {
            Update(e.ArticleId, x =>
                x.Name = e.Name);
        }

        public void Process(ImageAdded e)
        {
            if (!e.IsPreviewImage)
                return;

            Update(e.ArticleId, x =>
                x.PreviewImageFileName = e.FileName);            
        }

        public void Process(PreviewImageChanged e)
        {
            Update(e.ArticleId, x =>
                x.PreviewImageFileName = e.FileName);
        }

        public void Process(PricesChanged e)
        {
            Update(e.ArticleId, x =>
            {
                x.MemberPrice = e.MemberPrice;
                x.PublicPrice = e.PublicPrice;
            });
        }

        public void Process(StoreRenamed e)
        {
            Update(p => p.StoreId == e.StoreId, a =>
                a.StoreName = e.Name);
        }        
    }

    public class ShopItemsData
    {
        public virtual Guid ItemId { get; set; }
        public virtual int ItemShortId { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid OwnerGuid { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual int OwnerType { get; set; }
        public virtual Guid StoreId { get; set; }
        public virtual string StoreName { get; set; }
        public virtual string PreviewImageFileName { get; set; }

        public virtual ICollection<ShopItemsSortByPopularityProjectionRow> Popularities { get; set; }
    }
}