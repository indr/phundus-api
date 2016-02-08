namespace Phundus.Persistence.Inventory.Mappings
{
    using Extensions;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Inventory.Projections;

    public class ArticlesActionsProjectionRowMap : ClassMap<ArticlesActionsProjectionRow>
    {
        public ArticlesActionsProjectionRowMap()
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