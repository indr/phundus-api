namespace Phundus.Shop.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Inventory.Articles.Model;

    public class ShopItemFilesProjection : ReadModelBase<ShopItemFilesProjectionRow>, IStoredEventsConsumer
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
            if (!domainEvent.FileType.StartsWith("application/"))
                return;

            var row = Find(domainEvent.ArticleGuid, domainEvent.FileName) ?? CreateRow();
            row.ArticleGuid = domainEvent.ArticleGuid;
            row.ArticleId = domainEvent.ArticleIntegralId;
            row.FileLength = domainEvent.FileLength;
            row.FileName = domainEvent.FileName;
            row.FileType = domainEvent.FileType;

            SaveOrUpdate(row);
        }

        public void Process(ImageRemoved domainEvent)
        {
            var row = Find(domainEvent.ArticleGuid, domainEvent.FileName);
            if (row == null)
                return;
            Delete(row);
        }

        private ShopItemFilesProjectionRow Find(Guid articleGuid, string fileName)
        {
            return Session.QueryOver<ShopItemFilesProjectionRow>()
                .Where(p => p.ArticleGuid == articleGuid && p.FileName == fileName).SingleOrDefault();
        }
    }
}