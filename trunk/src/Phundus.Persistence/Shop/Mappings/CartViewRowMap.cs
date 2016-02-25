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

            Id(x => x.CartId, "CartGuid");
            Map(x => x.UserId, "UserGuid");

            HasMany<CartItemViewRow>(me => me.Items).KeyColumn("CartGuid").OrderBy("LessorName");
        }
    }
}