namespace Phundus.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Articles.Model;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Common.Projections;
    using Newtonsoft.Json;

    public interface IArticleActionsQueries
    {
        IEnumerable<ArticleActionData> GetActions(ArticleId articleId);
    }

    public class ArticleActionsProjection : ProjectionBase<ArticleActionData>, IArticleActionsQueries,
        IStoredEventsConsumer
    {
        public IEnumerable<ArticleActionData> GetActions(ArticleId articleId)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");

            return QueryOver().Where(p => p.ArticleId == articleId.Id).OrderBy(p => p.OccuredOnUtc).Desc.List();
        }

        public override void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private ArticleActionData Create(DomainEvent @event)
        {
            var row = new ArticleActionData();
            row.EventGuid = @event.EventGuid;
            row.Name = @event.GetType().Name;
            row.OccuredOnUtc = @event.OccuredOnUtc;
            return row;
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(ArticleCreated e)
        {
            var row = Create(e);
            row.OwnerId = e.Owner.OwnerId.Id;
            row.StoreId = e.StoreId;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                owner = e.Owner,
                storeId = e.StoreId,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                name = e.Name,
                grossStock = e.GrossStock,
                publicPrice = e.PublicPrice,
                memberPrice = e.MemberPrice
            });
            Insert(row);
        }

        private void Process(ArticleDeleted e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
            });
            Insert(row);
        }

        private void Process(ArticleDetailsChanged e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                name = e.Name,
                brand = e.Brand,
                color = e.Color
            });
            Insert(row);
        }

        private void Process(DescriptionChanged e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                description = e.Description
            });
            Insert(row);
        }

        private void Process(SpecificationChanged e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                specification = e.Specification
            });
            Insert(row);
        }

        private void Process(GrossStockChanged e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                oldGrossStock = e.OldGrossStock,
                newGrossStock = e.NewGrossStock
            });
            Insert(row);
        }

        private void Process(PricesChanged e)
        {
            var row = Create(e);
            row.OwnerId = Guid.Empty;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                publicPrice = e.PublicPrice,
                memberPrice = e.MemberPrice
            });
            Insert(row);
        }

        private void Process(ImageAdded e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                fileName = e.FileName
            });
            Insert(row);
        }

        private void Process(ImageRemoved e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                fileName = e.FileName
            });
            Insert(row);
        }

        private void Process(PreviewImageChanged e)
        {
            var row = Create(e);
            row.OwnerId = e.OwnerId;
            row.StoreId = Guid.Empty;
            row.ArticleId = e.ArticleId;
            row.SetData(new
            {
                initiator = e.Initiator,
                articleId = e.ArticleId,
                articleShortId = e.ArticleShortId,
                fileName = e.FileName
            });
            Insert(row);
        }
    }

    public class ArticleActionData
    {
        [JsonProperty("eventGuid")]
        public virtual Guid EventGuid { get; set; }

        [JsonProperty("occuredOnUtc")]
        public virtual DateTime OccuredOnUtc { get; set; }

        [JsonProperty("type")]
        public virtual string Name { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof (RawJsonConverter))]
        public virtual string JsonData { get; protected set; }

        [JsonProperty("ownerId")]
        public virtual Guid OwnerId { get; set; }

        [JsonProperty("storeId")]
        public virtual Guid StoreId { get; set; }

        [JsonProperty("articleId")]
        public virtual Guid ArticleId { get; set; }

        public virtual void SetData(object data)
        {
            var stringWriter = new StringWriter();
            var settings = new JsonSerializerSettings();
            JsonSerializer.Create(settings).Serialize(stringWriter, data);
            JsonData = stringWriter.GetStringBuilder().ToString();
        }
    }
}