namespace Phundus.Shop.Application
{
    using System;
    using System.Collections.Generic;

    public class ShopItemData
    {
        private ICollection<ShopItemDocumentData> _documents = new List<ShopItemDocumentData>();
        private ICollection<ShopItemImageData> _images = new List<ShopItemImageData>();

        public virtual Guid ArticleId { get; set; }
        public virtual int ArticleShortId { get; set; }
        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual string Name { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Color { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid LessorId { get; set; }
        public virtual string LessorName { get; set; }
        public virtual string LessorUrl { get; set; }
        public virtual LessorType LessorType { get; set; }
        public virtual Guid StoreId { get; set; }
        public virtual string StoreName { get; set; }
        public virtual string StoreUrl { get; set; }
        public virtual string Description { get; set; }
        public virtual string Specification { get; set; }

        public virtual ICollection<ShopItemDocumentData> Documents
        {
            get { return _documents; }
            protected set { _documents = value; }
        }

        public virtual ICollection<ShopItemImageData> Images
        {
            get { return _images; }
            protected set { _images = value; }
        }
    }

    public class ShopItemDocumentData
    {
        public virtual Guid DataId { get; set; }

        public virtual ShopItemData ShopItem { get; set; }

        public virtual string FileName { get; set; }
        public virtual string FileType { get; set; }
        public virtual long FileLength { get; set; }
    }

    public class ShopItemImageData
    {
        public virtual Guid DataId { get; set; }

        public virtual ShopItemData ShopItem { get; set; }

        public virtual string FileName { get; set; }
        public virtual string FileType { get; set; }
        public virtual long FileLength { get; set; }
    }
}