namespace Phundus.Persistence.Inventory.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Inventory.Projections;

    public class StoreRowMap : ClassMap<StoreRow>
    {
        public StoreRowMap()
        {
            SchemaAction.All();

            Table("Es_Inventory_Stores");

            Id(x => x.StoreId).GeneratedBy.Assigned();
            Map(x => x.OwnerId).Not.Nullable();
            Map(x => x.Address).Nullable();
            Map(x => x.OpeningHours).Nullable();
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
        }
    }
}