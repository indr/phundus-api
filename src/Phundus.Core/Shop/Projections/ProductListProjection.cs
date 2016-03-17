namespace Phundus.Shop.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application;
    using Common;
    using Common.Eventing;
    using Common.Projecting;
    using Inventory.Articles.Model;
    using Inventory.Stores.Model;
    using Orders.Model;

    public class ProductListProjection : ProjectionBase<ProductListData>,
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
                        popularity = new ProductPopularityData(x, articleId, month);
                        x.Popularities.Add(popularity);
                    }
                    popularity.Value += 1;
                });
            }
        }
    }
}