namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Application.Data;
    using FluentNHibernate.Mapping;

    public class QuantityInInventoryDataMap : ClassMap<QuantityInInventoryData>
    {
        public QuantityInInventoryDataMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.OrganizationId);
            Map(x => x.StockId);
            Map(x => x.ArticleId);

            Map(x => x.AsOfUtc);
            Map(x => x.Change);
            Map(x => x.Total);
            Map(x => x.Comment);
        }
    }
}