namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;

    public class CartMap : ClassMap<Cart>
    {
        public CartMap()
        {
            SchemaAction.Validate();

            Table("Cart");
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            References(x => x.Customer).Column("CustomerId");

            HasMany(x => x.Items).AsSet()
                .KeyColumn("CartId").Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }
}