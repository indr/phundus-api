namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Queries;

    public class CartItemViewRowMap : ClassMap<CartItemViewRow>
    {
        public CartItemViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_Shop_CartItems");

            Id(x => x.CartItemGuid, "CartItemGuid");
            Map(x => x.Position, "Position");
            Map(x => x.ArticleId, "Article_ArticleId");
            Map(x => x.Text, "Article_Name");
            Map(x => x.FromUtc, "FromUtc").CustomType<UtcDateTimeType>();
            Map(x => x.ToUtc, "ToUtc").CustomType<UtcDateTimeType>();
            Map(x => x.Days, "Days");
            Map(x => x.Quantity, "Quantity");
            Map(x => x.UnitPricePerWeek, "Article_UnitPricePerWeek");
            Map(x => x.ItemTotal, "ItemTotal");

            Map(x => x.OwnerGuid, "Article_Owner_OwnerId");
            Map(x => x.OwnerName, "Article_Owner_Name");
        }
    }
}