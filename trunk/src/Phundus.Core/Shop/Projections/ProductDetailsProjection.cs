namespace Phundus.Shop.Projections
{
    using System;
    using System.Linq;
    using Application;
    using Common;
    using Common.Eventing;
    using Common.Projecting;
    using Inventory.Articles.Model;
    using Inventory.Model.Articles;
    using Inventory.Stores.Model;

    public class ProductDetailsProjection : ProjectionBase<ProductDetailsData>,
        ISubscribeTo<ArticleCreated>,
        ISubscribeTo<ArticleDeleted>,
        ISubscribeTo<ArticleDetailsChanged>,
        ISubscribeTo<DescriptionChanged>,
        ISubscribeTo<SpecificationChanged>,
        ISubscribeTo<PricesChanged>,
        ISubscribeTo<StoreRenamed>,
        ISubscribeTo<ImageAdded>,
        ISubscribeTo<ImageRemoved>,
        ISubscribeTo<ProductTagged>,
        ISubscribeTo<ProductUntagged>

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
                    image.ProductDetails = null;
                    x.Images.Remove(image);
                    return;
                }

                var document = x.Documents.SingleOrDefault(p => p.FileName == e.FileName);
                if (document != null)
                {
                    document.ProductDetails = null;
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

                var document = new ProductDetailsDocumentData
                {
                    ProductDetails = x,
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

                var image = new ProductDetailsImageData
                {
                    ProductDetails = x,
                    FileLength = e.FileLength,
                    FileName = e.FileName,
                    FileType = e.FileType
                };

                x.Images.Add(image);
            });
        }


        public void Handle(ProductTagged e)
        {
            if (String.IsNullOrWhiteSpace(e.TagName))
                return;

            Update(e.ArticleId, x =>
                x.Tags.Add(e.TagName));
        }

        public void Handle(ProductUntagged e)
        {
            if (String.IsNullOrWhiteSpace(e.TagName))
                return;

            Update(e.ArticleId, x =>
                x.Tags.Remove(e.TagName));
        }
    }
}