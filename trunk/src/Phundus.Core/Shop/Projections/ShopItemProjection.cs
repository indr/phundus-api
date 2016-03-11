namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Eventing;
    using Common.Projecting;
    using Inventory.Articles.Model;
    using Inventory.Stores.Model;

    public class ShopItemProjection : ProjectionBase<ShopItemData>,
        ISubscribeTo<ArticleCreated>,
        ISubscribeTo<ArticleDeleted>,
        ISubscribeTo<ArticleDetailsChanged>,
        ISubscribeTo<DescriptionChanged>,
        ISubscribeTo<SpecificationChanged>,
        ISubscribeTo<PricesChanged>,
        ISubscribeTo<StoreRenamed>,
        ISubscribeTo<ImageAdded>,
        ISubscribeTo<ImageRemoved>

    {
        public void Handle(ArticleCreated e)
        {
            if (e.ArticleId == Guid.Empty)
                return;

            Insert(x =>
            {
                x.ArticleId = e.ArticleId;
                x.ArticleShortId = e.ArticleShortId;
                x.CreatedAtUtc = e.OccuredOnUtc;

                x.LessorId = e.Owner.OwnerId.Id;
                x.LessorType = LessorTypeConvertor.From(e.Owner.Type.ToString());
                x.LessorName = e.Owner.Name;
                x.LessorUrl = e.Owner.Name.ToFriendlyUrl();

                x.StoreId = e.StoreId;
                x.StoreName = e.StoreName;
                x.StoreUrl = x.LessorUrl + "/" + x.StoreName.ToFriendlyUrl();

                x.Name = e.Name;
                x.PublicPrice = e.PublicPrice;
                x.MemberPrice = e.MemberPrice;
            });
        }

        public void Handle(ArticleDeleted e)
        {
            Delete(e.ArticleId);
        }

        public void Handle(ArticleDetailsChanged domainEvent)
        {
            Update(domainEvent.ArticleId, x =>
            {
                x.Name = domainEvent.Name;
                x.Brand = domainEvent.Brand;
                x.Color = domainEvent.Color;
            });
        }

        public void Handle(DescriptionChanged e)
        {
            Update(e.ArticleId, x =>
                x.Description = e.Description);
        }

        public void Handle(ImageAdded domainEvent)
        {
            if (domainEvent.FileType.StartsWith("application/"))
                ProcessDocumentAdded(domainEvent);
            else
                ProcessImageAdded(domainEvent);
        }

        public void Handle(ImageRemoved e)
        {
            Update(e.ArticleId, x =>
            {
                var image = x.Images.SingleOrDefault(p => p.FileName == e.FileName);
                if (image != null)
                {
                    image.ShopItem = null;
                    x.Images.Remove(image);
                    return;
                }

                var document = x.Documents.SingleOrDefault(p => p.FileName == e.FileName);
                if (document != null)
                {
                    document.ShopItem = null;
                    x.Documents.Remove(document);
                }
            });
        }

        public void Handle(PricesChanged e)
        {
            Update(e.ArticleId, x =>
            {
                x.PublicPrice = e.PublicPrice;
                x.MemberPrice = e.MemberPrice;
            });
        }

        public void Handle(SpecificationChanged e)
        {
            Update(e.ArticleId, x =>
                x.Specification = e.Specification);
        }

        public void Handle(StoreRenamed e)
        {
            Update(p => p.StoreId == e.StoreId, x =>
            {
                x.StoreName = e.Name;
                x.StoreUrl = x.LessorUrl + "/" + e.Name.ToFriendlyUrl();
            });
        }

        private void ProcessDocumentAdded(ImageAdded e)
        {
            Update(e.ArticleId, x =>
            {
                if (x.Documents.SingleOrDefault(p => p.FileName == e.FileName) != null)
                {
                    throw new Exception(
                        String.Format("Could not process event {0}. Document with file name {1} already exists.",
                            e.EventGuid, e.FileName));
                }

                var document = new ShopItemDocumentData
                {
                    ShopItem = x,
                    FileLength = e.FileLength,
                    FileName = e.FileName,
                    FileType = e.FileType
                };

                x.Documents.Add(document);
            });
        }

        private void ProcessImageAdded(ImageAdded e)
        {
            Update(e.ArticleId, x =>
            {
                if (x.Images.SingleOrDefault(p => p.FileName == e.FileName) != null)
                {
                    throw new Exception(
                        String.Format("Could not process event {0}. Image with file name {1} already exists.",
                            e.EventGuid, e.FileName));
                }

                var image = new ShopItemImageData
                {
                    ShopItem = x,
                    FileLength = e.FileLength,
                    FileName = e.FileName,
                    FileType = e.FileType
                };

                x.Images.Add(image);
            });
        }
    }

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