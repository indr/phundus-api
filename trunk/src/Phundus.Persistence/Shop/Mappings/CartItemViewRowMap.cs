namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Queries;

    public class CartItemViewRowMap : ClassMap<CartItemViewRow>
    {
        public CartItemViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_Shop_CartItems");

            Id(x => x.CartItemId, "CartItemId");
            Map(x => x.CartItemGuid, "CartItemGuid");
            Map(x => x.ArticleId, "ArticleId");
            Map(x => x.Text, "Text");
            Map(x => x.FromUtc, "FromUtc");
            Map(x => x.ToUtc, "ToUtc");
            Map(x => x.Quantity, "Quantity");
        }
    }
}