namespace Phundus.Shop.Projections
{
    using System;

    public class ShopItemProjectionRow
    {
        public virtual Guid RowId { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual Guid ArticleGuid { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid OwnerGuid { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual int OwnerType { get; set; }
        public virtual string Description { get; set; }
        public virtual string Specification { get; set; }
    }
}