namespace Phundus.Inventory.Projections
{
    using System;
    using System.IO;
    using Articles.Model;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Projecting;
    using Common.Projections;
    using Newtonsoft.Json;

    public class ArticleActionsProjection : ProjectionBase<ArticleActionData>,
        ISubscribeTo<ArticleCreated>,
        ISubscribeTo<ArticleDeleted>,
        ISubscribeTo<ArticleDetailsChanged>,
        ISubscribeTo<DescriptionChanged>,
        ISubscribeTo<SpecificationChanged>,
        ISubscribeTo<PricesChanged>,
        ISubscribeTo<GrossStockChanged>,
        ISubscribeTo<ImageAdded>,
        ISubscribeTo<ImageRemoved>,
        ISubscribeTo<PreviewImageChanged>
    {
        public void Handle(ArticleCreated e)
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

        public void Handle(ArticleDeleted e)
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

        public void Handle(ArticleDetailsChanged e)
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

        public void Handle(DescriptionChanged e)
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

        public void Handle(GrossStockChanged e)
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

        public void Handle(ImageAdded e)
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

        public void Handle(ImageRemoved e)
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

        public void Handle(PreviewImageChanged e)
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

        public void Handle(PricesChanged e)
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

        public void Handle(SpecificationChanged e)
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

        private static ArticleActionData Create(DomainEvent @event)
        {
            var row = new ArticleActionData();
            row.EventGuid = @event.EventGuid;
            row.Name = @event.GetType().Name;
            row.OccuredOnUtc = @event.OccuredOnUtc;
            return row;
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