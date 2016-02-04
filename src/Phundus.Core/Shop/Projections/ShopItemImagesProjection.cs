namespace Phundus.Shop.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;

    public class ShopItemImagesProjection : ReadModelBase<ShopItemImagesProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic)domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(ImageAdded domainEvent)
        {
            if (!domainEvent.FileType.StartsWith("image/"))
                return;

            var row = Find(domainEvent.ArticleGuid, domainEvent.FileName) ?? CreateRow();
            row.ArticleGuid = domainEvent.ArticleGuid;
            row.ArticleId = domainEvent.ArticleIntegralId;
            row.FileLength = domainEvent.FileLength;
            row.FileName = domainEvent.FileName;
            row.FileType = domainEvent.FileType;
            Session.SaveOrUpdate(row);
            Session.Flush();
        }

        public void Process(ImageRemoved domainEvent)
        {            
            var row = Find(domainEvent.ArticleGuid, domainEvent.FileName);
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
}