namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;

    public class CartMap : ClassMap<Cart>
    {
        public CartMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_Cart");
            Id(x => x.Id, "CartId").GeneratedBy.Native();
            Map(x => x.CartGuid, "CartGuid");
            Version(x => x.Version);

            Map(x => x.CustomerId, "UserId");
            Map(x => x.UserGuid, "UserGuid");

            HasMany(x => x.Items).AsSet()
                .KeyColumn("CartId").Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }
}