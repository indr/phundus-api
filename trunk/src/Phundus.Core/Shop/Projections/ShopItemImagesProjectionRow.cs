namespace Phundus.Shop.Projections
{
    using System;

    public class ShopItemImagesProjectionRow
    {
        public virtual Guid RowId { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual Guid ArticleGuid { get; set; }
        public virtual string FileName { get; set; }
        public virtual string FileType { get; set; }
        public virtual long FileLength { get; set; }
    }
}