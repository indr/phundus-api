namespace Phundus.Inventory.Infrastructure.Persistence.Mappings
{
    using Common.Infrastructure.Persistence;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Projections;

    public class ArticleActionDataMap : ClassMap<ArticleActionData>
    {
        public ArticleActionDataMap()
        {
            SchemaAction.All();
            Table("Es_Inventory_Articles_Actions");

            Id(x => x.EventGuid).GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.OccuredOnUtc).CustomType<UtcDateTimeType>();

            Map(x => x.OwnerId);
            Map(x => x.StoreId);
            Map(x => x.ArticleId);
            Map(x => x.JsonData).WithMaxSize();
        }
    }
}