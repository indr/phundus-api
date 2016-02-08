namespace Phundus.Inventory.Projections
{
    using System;
    using Common.Projections;
    using Newtonsoft.Json;

    public class ArticlesActionsProjectionRow : ActionsProjectionRowBase
    {
        [JsonProperty("ownerId")]
        public virtual Guid OwnerId { get; set; }

        [JsonProperty("storeId")]
        public virtual Guid StoreId { get; set; }

        [JsonProperty("articleId")]
        public virtual Guid ArticleId { get; set; }
    }
}