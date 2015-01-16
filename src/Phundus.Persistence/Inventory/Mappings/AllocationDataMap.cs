namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Application.Data;
    using FluentNHibernate.Mapping;

    public class AllocationDataMap : ClassMap<AllocationData>
    {
        public AllocationDataMap()
        {
            Id(x => x.AllocationId).GeneratedBy.Assigned();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.ArticleId);
            Map(x => x.FromUtc);
            Map(x => x.OrganizationId);
            Map(x => x.Quantity);
            Map(x => x.Status);
            Map(x => x.StockId);
            Map(x => x.ToUtc);
        }
    }
}