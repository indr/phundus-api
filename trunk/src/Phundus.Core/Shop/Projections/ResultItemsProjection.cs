namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;

    public class ResultItemsProjection : ReadModelBase<ResultItemsProjectionRow>, IStoredEventsConsumer
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
            var row = CreateRow();
            row.ItemId = domainEvent.ArticleId;
            row.ItemShortId = domainEvent.ArticleShortId;
            row.CreatedAtUtc = domainEvent.OccuredOnUtc;
            row.MemberPrice = domainEvent.MemberPrice;
            row.Name = domainEvent.Name;
            row.OwnerGuid = domainEvent.Owner.OwnerId.Id;
            row.OwnerName = domainEvent.Owner.Name;
            row.OwnerType = (int) domainEvent.Owner.Type;
            row.PreviewImageFileName = "";
            row.PublicPrice = domainEvent.PublicPrice;
            Session.Save(row);
            Session.Flush();
        }

        public void Process(ArticleDetailsChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleId);
            row.Name = domainEvent.Name;
        }

        public void Process(ArticleDeleted domainEvent)
        {
            var row = Find(domainEvent.ArticleId);
            Delete(row);
        }

        public void Process(ImageAdded domainEvent)
        {
            if (!domainEvent.IsPreviewImage)
                return;

            var row = Find(domainEvent.ArticleId);
            row.PreviewImageFileName = domainEvent.FileName;
        }

        public void Process(PreviewImageChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleId);
            row.PreviewImageFileName = domainEvent.FileName;
        }

        public void Process(PricesChanged domainEvent)
        {
            var row = Find(domainEvent.ArticleId);
            row.MemberPrice = domainEvent.MemberPrice;
            row.PublicPrice = domainEvent.PublicPrice;
        }

        private ResultItemsProjectionRow Find(Guid articleGuid)
        {
            return Session.QueryOver<ResultItemsProjectionRow>()
                .Where(p => p.ItemId == articleGuid).SingleOrDefault();
        }
    }

    public class ResultItemsProjectionRow
    {
        public virtual Guid RowId { get; set; }

        public virtual Guid ItemId { get; set; }
        public virtual int ItemShortId { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid OwnerGuid { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual int OwnerType { get; set; }
        public virtual string PreviewImageFileName { get; set; }

        public virtual ICollection<ShopItemsSortByPopularityProjectionRow> Popularities { get; set; }
    }
}