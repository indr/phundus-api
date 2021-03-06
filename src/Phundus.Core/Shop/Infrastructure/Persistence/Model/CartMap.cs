namespace Phundus.Shop.Infrastructure.Persistence.Model
{
    using FluentNHibernate.Mapping;
    using Shop.Model;

    public class CartMap : ClassMap<Cart>
    {
        public CartMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_Cart");

            CompositeId(x => x.Id)
                .KeyProperty(kp => kp.Id, "CartGuid");
            Version(x => x.Version);

            Component(x => x.UserId, a =>
                a.Map(x => x.Id, "UserGuid"));

            HasMany(x => x.Items).AsSet()
                .KeyColumn("CartGuid").Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }
}