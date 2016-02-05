namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;

    public class ShopItemProjectionRow
    {
        public virtual Guid ArticleGuid { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Color { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid LessorId { get; set; }
        public virtual string LessorName { get; set; }
        public virtual int LessorType { get; set; }
        public virtual string Description { get; set; }
        public virtual string Specification { get; set; }

        public virtual ICollection<ShopItemFilesProjectionRow> Documents { get; set; }
        public virtual ICollection<ShopItemImagesProjectionRow> Images { get; set; }
    }
}