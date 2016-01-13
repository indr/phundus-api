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

            Id(x => x.CartId, "CartId");
            Map(x => x.CartGuid, "CartGuid");
            Map(x => x.UserId, "UserId");
            Map(x => x.UserGuid, "UserGuid");

            HasMany<CartItemViewRow>(me => me.Items).KeyColumn("CartId").OrderBy("Article_Owner_Name");
        }
    }
}