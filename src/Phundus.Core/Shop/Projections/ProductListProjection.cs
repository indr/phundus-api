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
    using Orders.Model;

    public class ProductListProjection : ProjectionBase<ShopItemsData>,
        ISubscribeTo<ArticleCreated>,
        ISubscribeTo<ArticleDeleted>,
        ISubscribeTo<ArticleDetailsChanged>,
        ISubscribeTo<ImageAdded>,
        ISubscribeTo<PreviewImageChanged>,
        ISubscribeTo<PricesChanged>,
        ISubscribeTo<StoreRenamed>,
        ISubscribeTo<OrderPlaced>
    {
        public void Handle(ArticleCreated e)
        {
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
                x.StoreUrl = x.LessorUrl + "/" + e.StoreName.ToFriendlyUrl();

                x.Name = e.Name;
                x.PublicPrice = e.PublicPrice;
                x.MemberPrice = e.MemberPrice;
                x.PreviewImageFileName = "";
            });
        }

        public void Handle(ArticleDeleted e)
        {
            Delete(e.ArticleId);
        }

        public void Handle(ArticleDetailsChanged e)
        {
            Update(e.ArticleId, x =>
                x.Name = e.Name);
        }

        public void Handle(ImageAdded e)
        {
            if (!e.IsPreviewImage)
                return;

            Update(e.ArticleId, x =>
                x.PreviewImageFileName = e.FileName);
        }

        public void Handle(OrderPlaced domainEvent)
        {
            if (domainEvent.Items == null)
                return;

            ProcessItems(domainEvent.Items);
        }

        public void Handle(PreviewImageChanged e)
        {
            Update(e.ArticleId, x =>
                x.PreviewImageFileName = e.FileName);
        }

        public void Handle(PricesChanged e)
        {
            Update(e.ArticleId, x =>
            {
                x.MemberPrice = e.MemberPrice;
                x.PublicPrice = e.PublicPrice;
            });
        }

        public void Handle(StoreRenamed e)
        {
            Update(p => p.StoreId == e.StoreId, x =>
            {
                x.StoreName = e.Name;
                x.StoreUrl = x.LessorUrl + "/" + e.Name.ToFriendlyUrl();
            });
        }

        private void ProcessItems(IEnumerable<OrderEventLine> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            foreach (var item in items)
                ProcessItem(item);
        }

        private void ProcessItem(OrderEventLine line)
        {
            if (line == null) throw new ArgumentNullException("line");
            if (line.ArticleId == Guid.Empty)
                return;

            var articleId = line.ArticleId;
            var from = new DateTime(line.FromUtc.Year, line.FromUtc.Month, 1);
            var to = new DateTime(line.ToUtc.Year, line.ToUtc.Month, 1).AddMonths(1);

            for (var firstOf = from.Date; firstOf < to; firstOf = firstOf.AddMonths(1))
            {
                var month = firstOf.Month;

                Update(articleId, x =>
                {
                    var popularity = x.Popularities.SingleOrDefault(p => p.Month == month);
                    if (popularity == null)
                    {
                        popularity = new ShopItemsPopularityData(x, articleId, month);
                        x.Popularities.Add(popularity);
                    }
                    popularity.Value += 1;
                });
            }
        }
    }

    public class ShopItemsData
    {
        private ICollection<ShopItemsPopularityData> _popularities = new List<ShopItemsPopularityData>();

        public virtual Guid ArticleId { get; set; }
        public virtual int ArticleShortId { get; set; }

        public virtual Guid LessorId { get; set; }
        public virtual LessorType LessorType { get; set; }
        public virtual string LessorName { get; set; }
        public virtual string LessorUrl { get; set; }

        public virtual Guid StoreId { get; set; }
        public virtual string StoreName { get; set; }
        public virtual string StoreUrl { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual string PreviewImageFileName { get; set; }

        public virtual ICollection<ShopItemsPopularityData> Popularities
        {
            get { return _popularities; }
            protected set { _popularities = value; }
        }
    }

    public class ShopItemsPopularityData
    {
        private Guid _articleId;
        private int _month;
        private ShopItemsData _shopItem;

        public ShopItemsPopularityData(ShopItemsData shopItem, Guid articleId, int month)
        {
            _shopItem = shopItem;
            _articleId = articleId;
            _month = month;
        }

        protected ShopItemsPopularityData()
        {
        }

        public virtual Guid RowId { get; protected set; }

        public virtual ShopItemsData ShopItem
        {
            get { return _shopItem; }
            protected set { _shopItem = value; }
        }

        public virtual Guid ArticleId
        {
            get { return _articleId; }
            protected set { _articleId = value; }
        }

        public virtual int Month
        {
            get { return _month; }
            protected set { _month = value; }
        }

        public virtual int Value { get; set; }
    }
}