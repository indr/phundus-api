namespace Phundus.Persistence.Shop.Mappings
{
    using System.Security.Cryptography.X509Certificates;
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

            HasMany<CartItemViewRow>(me => me.CartId);
        }

        
    }

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