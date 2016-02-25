namespace Phundus.Shop.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;

    public class ShopItemImagesProjection : ProjectionBase<ShopItemImagesProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic)e);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(ImageAdded domainEvent)
        {
            if (!domainEvent.FileType.StartsWith("image/"))
                return;

            var row = Find(domainEvent.ArticleId, domainEvent.FileName) ?? new ShopItemImagesProjectionRow();
            row.ArticleGuid = domainEvent.ArticleId;
            row.ArticleId = domainEvent.ArticleShortId;
            row.FileLength = domainEvent.FileLength;
            row.FileName = domainEvent.FileName;
            row.FileType = domainEvent.FileType;

            InsertOrUpdate(row);
        }

        public void Process(ImageRemoved domainEvent)
        {            
            var row = Find(domainEvent.ArticleId, domainEvent.FileName);
            if (row == null)
                return;
            Delete(row);
        }

        private ShopItemImagesProjectionRow Find(Guid articleGuid, string fileName)
        {
            return Session.QueryOver<ShopItemImagesProjectionRow>()
                .Where(p => p.ArticleGuid == articleGuid && p.FileName == fileName).SingleOrDefault();
        }
    }

    public class ShopItemImagesProjectionRow
    {
        public virtual Guid RowGuid { get; set; }
        public virtual Guid ArticleGuid { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual string FileName { get; set; }
        public virtual string FileType { get; set; }
        public virtual long FileLength { get; set; }
    }
}