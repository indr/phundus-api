namespace Phundus.Inventory.Infrastructure.Persistence.Mappings
{
    using Application;
    using Common.Infrastructure.Persistence;
    using FluentNHibernate.Mapping;

    public class StoreDetailsDataMap : ClassMap<StoreDetailsData>
    {
        public StoreDetailsDataMap()
        {
            SchemaAction.All();

            Table("Es_Inventory_StoreDetails");

            Id(x => x.StoreId).GeneratedBy.Assigned();
            Map(x => x.OwnerId).Not.Nullable();
            Map(x => x.OwnerType).Not.Nullable();
            Map(x => x.Name).Nullable();

            Map(x => x.Line1).Nullable();
            Map(x => x.Line2).Nullable();
            Map(x => x.Street).Nullable();
            Map(x => x.Postcode).Nullable();
            Map(x => x.City).Nullable();
            Map(x => x.PostalAddress).WithMaxSize().Nullable();

            Map(x => x.EmailAddress).Nullable();
            Map(x => x.PhoneNumber).Nullable();

            Map(x => x.OpeningHours).Nullable();
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
        }
    }
}