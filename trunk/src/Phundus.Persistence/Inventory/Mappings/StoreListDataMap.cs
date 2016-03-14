namespace Phundus.Persistence.Inventory.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Inventory.Projections;

    public class StoreListDataMap : ClassMap<StoreListData>
    {
        public StoreListDataMap()
        {
            SchemaAction.All();

            Table("Es_Inventory_StoreList");

            Id(x => x.StoreId).GeneratedBy.Assigned();
            Map(x => x.OwnerId).Not.Nullable();
            Map(x => x.OwnerType).Not.Nullable();
            Map(x => x.Name).Nullable();            
        }
    }
}