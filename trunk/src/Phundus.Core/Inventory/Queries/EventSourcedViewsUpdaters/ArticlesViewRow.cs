namespace Phundus.Inventory.Queries.EventSourcedViewsUpdaters
{
    using System;
    using Articles.Model;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;

    public class EsArticlesUpdater : NHibernateReadModelBase<ArticlesViewRow>, IDomainEventHandler
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

            var row = new ArticlesViewRow();
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

            Session.SaveOrUpdate(row);
            Session.Flush();
        }

        public void Process(ArticleDeleted domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            Session.Delete(row);
        }

        public void Process(ArticleDetailsChanged domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            row.Name = domainEvent.Name;
            row.Brand = domainEvent.Brand;
            row.Color = domainEvent.Color;
            Session.SaveOrUpdate(row);
        }

        public void Process(GrossStockChanged domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            row.GrossStock = domainEvent.NewGrossStock;
            Session.SaveOrUpdate(row);
        }

        public void Process(DescriptionChanged domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            row.Description = domainEvent.Description;
            Session.SaveOrUpdate(row);
        }

        public void Process(SpecificationChanged domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            row.Specification = domainEvent.Specification;
            Session.SaveOrUpdate(row);
        }

        public void Process(PricesChanged domainEvent)
        {
            var row = FindByArticleGuid(domainEvent.ArticleGuid);
            row.PublicPrice = domainEvent.PublicPrice;
            row.MemberPrice = domainEvent.MemberPrice;
            Session.SaveOrUpdate(row);
        }

        private ArticlesViewRow FindByArticleGuid(Guid articleGuid)
        {
            return Session.QueryOver<ArticlesViewRow>()
                .Where(p => p.ArticleGuid == articleGuid)
                .SingleOrDefault();
        }
    }

    public class ArticlesViewRow
    {
        public virtual Guid RowGuid { get; set; }

        public virtual int ArticleId { get; set; }
        public virtual Guid ArticleGuid { get; set; }
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