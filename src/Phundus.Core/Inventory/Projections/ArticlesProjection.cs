namespace Phundus.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Articles.Model;
    using Common.Eventing;
    using Common.Projecting;
    using Model;
    using Model.Articles;

    public class ArticlesProjection : ProjectionBase<ArticleData>,
        ISubscribeTo<ArticleCreated>,
        ISubscribeTo<ArticleDeleted>,
        ISubscribeTo<ArticleDetailsChanged>,
        ISubscribeTo<GrossStockChanged>,
        ISubscribeTo<DescriptionChanged>,
        ISubscribeTo<SpecificationChanged>,
        ISubscribeTo<PricesChanged>,
        ISubscribeTo<ProductTagged>,
        ISubscribeTo<ProductUntagged>
    {
        public void Handle(ArticleCreated e)
        {
            if (e.ArticleId == Guid.Empty)
                return;

            Insert(row =>
            {
                row.ArticleId = e.ArticleId;
                row.ArticleShortId = e.ArticleShortId;
                row.CreatedAtUtc = e.OccuredOnUtc;
                row.GrossStock = e.GrossStock;
                row.Name = e.Name;
                row.OwnerGuid = e.Owner.OwnerId.Id;
                row.OwnerName = e.Owner.Name;
                row.OwnerType = e.Owner.Type;
                row.StoreId = e.StoreId;
                row.PublicPrice = e.PublicPrice;
                row.MemberPrice = e.MemberPrice;
            });
        }

        public void Handle(ArticleDeleted e)
        {
            Delete(e.ArticleId);
        }

        public void Handle(ArticleDetailsChanged e)
        {
            Update(e.ArticleId, r =>
            {
                r.Name = e.Name;
                r.Brand = e.Brand;
                r.Color = e.Color;
            });
        }

        public void Handle(DescriptionChanged e)
        {
            Update(e.ArticleId, r => { r.Description = e.Description; });
        }

        public void Handle(GrossStockChanged e)
        {
            Update(e.ArticleId, r => { r.GrossStock = e.NewGrossStock; });
        }

        public void Handle(PricesChanged e)
        {
            Update(e.ArticleId, r =>
            {
                r.PublicPrice = e.PublicPrice;
                r.MemberPrice = e.MemberPrice;
            });
        }

        public void Handle(SpecificationChanged e)
        {
            Update(e.ArticleId, r => { r.Specification = e.Specification; });
        }

        public void Handle(ProductTagged e)
        {
            Update(e.ArticleId, x =>
                x.Tags.Add(e.TagName));
        }

        public void Handle(ProductUntagged e)
        {
            Update(e.ArticleId, x =>
                x.Tags.Remove(e.TagName));
        }
    }

    public class ArticleData
    {
        private ICollection<string> _tags = new Collection<string>();

        public virtual Guid ArticleId { get; set; }
        public virtual int ArticleShortId { get; set; }
        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual Guid OwnerGuid { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual OwnerType OwnerType { get; set; }
        public virtual Guid StoreId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Brand { get; set; }
        public virtual string Color { get; set; }
        public virtual string Description { get; set; }
        public virtual string Specification { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual int GrossStock { get; set; }

        public virtual ICollection<string> Tags
        {
            get { return _tags; }
            protected set { _tags = value; }
        }
    }
}