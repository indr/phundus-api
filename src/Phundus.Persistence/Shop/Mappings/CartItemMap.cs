namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;

    public class CartItemMap : ClassMap<CartItem>
    {
        public CartItemMap()
        {
            SchemaAction.Validate();

            Table("CartItem");
            Id(x => x.Id).GeneratedBy.HiLo("1000");
            Version(x => x.Version);

            References(x => x.Cart, "CartId");

            Map(x => x.Quantity, "Quantity");
            Map(x => x.From, "[From]");
            Map(x => x.To, "[To]");

            References(x => x.Article, "ArticleId");
        }
    }
}