namespace Phundus.Persistence.Inventory.Mappings
{
    using Core.Inventory.Stores.Model;
    using FluentNHibernate.Mapping;

    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            SchemaAction.Validate();

            Table("Dm_Inventory_Store");
            CompositeId(x => x.Id).KeyProperty(e => e.Id, "StoreId");            
            Version(x => x.Version);
            Map(x => x.CreatedAtUtc).Not.Update();
            Map(x => x.ModifiedAtUtc);

            Map(x => x.Address);
            Component(x => x.Coordinate, c =>
            {
                c.Map(x => x.Latitude, "Coordinate_Latitude");
                c.Map(x => x.Longitude, "Coordinate_Longitude");
            });
            Map(x => x.OpeningHours);
            Component(x => x.Owner, c =>
            {
                c.Component(x => x.OwnerId, d => d.Map(x => x.Id, "Owner_OwnerId"));
                c.Map(x => x.Name, "Owner_Name");
            });
        }
    }
}