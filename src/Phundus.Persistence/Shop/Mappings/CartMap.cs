namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Orders.Model;

    public class CartMap : ClassMap<Cart>
    {
        public CartMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_Cart");

            CompositeId(x => x.Id).KeyProperty(kp => kp.Id, "CartGuid");
            Version(x => x.Version);

            Map(x => x.UserGuid, "UserGuid");

            HasMany(x => x.Items).AsSet()
                .KeyColumn("CartGuid").Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }
}