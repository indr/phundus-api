namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Queries;

    public class CartViewRowMap : ClassMap<CartViewRow>
    {
        public CartViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_Shop_Carts");

            Id(x => x.CartGuid, "CartGuid");
            Map(x => x.UserGuid, "UserGuid");

            HasMany<CartItemViewRow>(me => me.Items).KeyColumn("CartGuid").OrderBy("Article_Owner_Name");
        }
    }
}