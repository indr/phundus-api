namespace Phundus.Inventory.Projections
{
    using System;
    using Articles.Model;
    using Common.Domain.Model;
    using Common.Notifications;

    public class ArticlesActionsProjection : ActionsProjectionBase<ArticlesActionsProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        private void Process(ArticleCreated domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.Owner.OwnerId.Id;
            row.StoreId = domainEvent.StoreId;
            row.ArticleId = domainEvent.ArticleGuid;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                owner = domainEvent.Owner,
                storeId = domainEvent.StoreId,
                articleId = domainEvent.ArticleGuid,
                articleShortId = domainEvent.ArticleId,
                name = domainEvent.Name,
                grossStock = domainEvent.GrossStock,
                publicPrice = domainEvent.PublicPrice,
                memberPrice = domainEvent.MemberPrice
            });
            Insert(row);
        }

        private void Process(ArticleDeleted domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,                
            });
            Insert(row);
        }

        private void Process(ArticleDetailsChanged domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                name = domainEvent.Name,
                brand = domainEvent.Brand,
                color = domainEvent.Color
            });
            Insert(row);
        }

        private void Process(DescriptionChanged domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                description = domainEvent.Description
            });
            Insert(row);
        }

        private void Process(SpecificationChanged domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                specification = domainEvent.Specification
            });
            Insert(row);
        }

        private void Process(GrossStockChanged domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                oldGrossStock = domainEvent.OldGrossStock,
                newGrossStock = domainEvent.NewGrossStock
            });
            Insert(row);
        }

        private void Process(PricesChanged domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = Guid.Empty;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                publicPrice = domainEvent.PublicPrice,
                memberPrice = domainEvent.MemberPrice
            });
            Insert(row);
        }

        private void Process(ImageAdded domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                fileName = domainEvent.FileName
            });
            Insert(row);
        }

        private void Process(ImageRemoved domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                fileName = domainEvent.FileName
            });
            Insert(row);
        }

        private void Process(PreviewImageChanged domainEvent)
        {
            var row = CreateRow(domainEvent);
            row.OwnerId = domainEvent.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = domainEvent.ArticleId;
            row.SetData(new
            {
                initiator = domainEvent.Initiator,
                articleId = domainEvent.ArticleId,
                articleShortId = domainEvent.ArticleShortId,
                fileName = domainEvent.FileName
            });
            Insert(row);
        }
    }
}