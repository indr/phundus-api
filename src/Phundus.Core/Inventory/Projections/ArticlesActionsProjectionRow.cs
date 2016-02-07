namespace Phundus.Inventory.Projections
{
    using System;
    using Dashboard.Projections;

    public class ArticlesActionsProjectionRow : ActionsProjectionRowBase
    {
        public virtual Guid OwnerId { get; set; }
        public virtual Guid StoreId { get; set; }
        public virtual Guid ArticleId { get; set; }
    }
}