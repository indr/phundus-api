namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Application.Data;
    using FluentNHibernate.Mapping;

    public class QuantityAvailableDataMap : ClassMap<QuantityAvailableData>
    {
        public QuantityAvailableDataMap()
        {
            SchemaAction.All();
            Table("Proj_QuantityAvailableData");

            Id(x => x.Id).GeneratedBy.GuidComb();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.OrganizationId);
            Map(x => x.ArticleId);
            Map(x => x.StockId);
            Map(x => x.AsOfUtc);
            Map(x => x.Quantity);
        }
    }
}