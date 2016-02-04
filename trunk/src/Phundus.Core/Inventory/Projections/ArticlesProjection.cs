﻿namespace Phundus.Inventory.Projections
{
    using System;
    using Articles.Model;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;

    public class ArticlesProjection : NHibernateReadModelBase<ArticlesProjectionRow>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        public void Process(DomainEvent domainEvent)
        {
            // Noop
        }

        public void Process(ArticleCreated domainEvent)
        {
            if (domainEvent.ArticleGuid == Guid.Empty)
                return;

            var row = new ArticlesProjectionRow();
            row.ArticleGuid = domainEvent.ArticleGuid;
            row.ArticleId = domainEvent.ArticleId;
            row.CreatedAtUtc = domainEvent.OccuredOnUtc;
            row.GrossStock = domainEvent.GrossStock;
            row.Name = domainEvent.Name;
            row.OwnerGuid = domainEvent.Owner.OwnerId.Id;
            row.OwnerName = domainEvent.Owner.Name;
            row.OwnerType = domainEvent.Owner.Type;
            row.StoreId = domainEvent.StoreId;
            row.PublicPrice = domainEvent.PublicPrice;
            row.MemberPrice = domainEvent.MemberPrice;

            SaveOrUpdate(row);
        }

        public void Process(ArticleDeleted domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            Session.Delete(row);
        }

        public void Process(ArticleDetailsChanged domainEvent)
        {
            Update(domainEvent.ArticleGuid, r =>
            {
                r.Name = domainEvent.Name;
                r.Brand = domainEvent.Brand;
                r.Color = domainEvent.Color;
            });
        }

        public void Process(GrossStockChanged domainEvent)
        {
            Update(domainEvent.ArticleGuid, r => { r.GrossStock = domainEvent.NewGrossStock; });
        }

        public void Process(DescriptionChanged domainEvent)
        {
            Update(domainEvent.ArticleGuid, r => { r.Description = domainEvent.Description; });
        }

        public void Process(SpecificationChanged domainEvent)
        {
            Update(domainEvent.ArticleGuid, r => { r.Specification = domainEvent.Specification; });
        }

        public void Process(PricesChanged domainEvent)
        {
            Update(domainEvent.ArticleGuid, r =>
            {
                r.PublicPrice = domainEvent.PublicPrice;
                r.MemberPrice = domainEvent.MemberPrice;
            });
        }

        private void Update(Guid articleGuid, Action<ArticlesProjectionRow> action)
        {
            var row = FindByArticleGuid(articleGuid);
            if (row == null)
                return;
            action(row);
            Session.SaveOrUpdate(row);
        }

        private ArticlesProjectionRow FindByArticleGuid(Guid articleGuid)
        {
            return Session.QueryOver<ArticlesProjectionRow>()
                .Where(p => p.ArticleGuid == articleGuid)
                .SingleOrDefault();
        }
    }
}