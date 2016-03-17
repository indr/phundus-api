namespace Phundus.Persistence.Shop.Projections
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Application;

    public class CartDataMap : ClassMap<CartData>
    {
        public CartDataMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_Shop_Carts");

            Id(x => x.CartId, "CartGuid");
            Map(x => x.UserId, "UserGuid");

            HasMany(me => me.Items).KeyColumn("CartGuid").OrderBy("LessorName");
        }
    }
}