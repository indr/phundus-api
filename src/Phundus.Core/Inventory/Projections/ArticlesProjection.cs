namespace Phundus.Inventory.Projections
{
    using System;
    using System.Collections.Generic;
    using Articles.Model;
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Cqrs;
    using Model;
    using NHibernate.Criterion;

    public interface IArticleQueries
    {
        ArticleData GetById(int id);

        IEnumerable<ArticleData> Query(InitiatorId initiatorId, OwnerId queryOwnerId, string query);
    }

    public class ArticlesProjection : ProjectionBase<ArticleData>, IArticleQueries, IStoredEventsConsumer
    {
        public ArticleData GetById(int id)
        {
            var result = SingleOrDefault(p => p.ArticleShortId == id);
            if (result == null)
                throw new NotFoundException(String.Format("Article {0} not found.", id));
            return result;
        }

        public IEnumerable<ArticleData> Query(InitiatorId initiatorId, OwnerId queryOwnerId, string query)
        {
            query = query == null ? "" : query.ToLowerInvariant();
            return QueryOver().Where(p => p.OwnerGuid == queryOwnerId.Id)
                .AndRestrictionOn(p => p.Name).IsInsensitiveLike(query, MatchMode.Anywhere)
                .List();
        }

        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(ArticleCreated e)
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

        private void Process(ArticleDeleted e)
        {
            Delete(e.ArticleId);
        }

        private void Process(ArticleDetailsChanged e)
        {
            Update(e.ArticleId, r =>
            {
                r.Name = e.Name;
                r.Brand = e.Brand;
                r.Color = e.Color;
            });
        }

        private void Process(GrossStockChanged e)
        {
            Update(e.ArticleId, r => { r.GrossStock = e.NewGrossStock; });
        }

        private void Process(DescriptionChanged e)
        {
            Update(e.ArticleId, r => { r.Description = e.Description; });
        }

        private void Process(SpecificationChanged e)
        {
            Update(e.ArticleId, r => { r.Specification = e.Specification; });
        }

        private void Process(PricesChanged e)
        {
            Update(e.ArticleId, r =>
            {
                r.PublicPrice = e.PublicPrice;
                r.MemberPrice = e.MemberPrice;
            });
        }
    }

    public class ArticleData
    {
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
    }
}