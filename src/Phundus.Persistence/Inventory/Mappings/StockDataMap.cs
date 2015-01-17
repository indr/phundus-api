namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Application.Data;
    using FluentNHibernate.Mapping;

    public class StockDataMap : ClassMap<StockData>
    {
        public StockDataMap()
        {
            SchemaAction.All();
            Table("Proj_StockData");

            Id(x => x.StockId).GeneratedBy.Assigned();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.ArticleId);
        }
    }
}