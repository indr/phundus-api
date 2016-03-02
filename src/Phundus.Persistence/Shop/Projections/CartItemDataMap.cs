namespace Phundus.Persistence.Shop.Projections
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Projections;

    public class CartItemDataMap : ClassMap<CartItemData>
    {
        public CartItemDataMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_Shop_CartItems");

            Id(x => x.CartItemGuid, "CartItemGuid");
            Map(x => x.Position, "Position");
            Map(x => x.ArticleId, "ArticleShortId");
            Map(x => x.ArticleGuid, "ArticleId");
            Map(x => x.Text, "Text");
            Map(x => x.FromUtc, "FromUtc").CustomType<UtcDateTimeType>();
            Map(x => x.ToUtc, "ToUtc").CustomType<UtcDateTimeType>();
            Map(x => x.Days, "Days");
            Map(x => x.Quantity, "Quantity");
            Map(x => x.UnitPricePerWeek, "UnitPricePerWeek");
            Map(x => x.ItemTotal, "ItemTotal");

            Map(x => x.OwnerGuid, "LessorId");
            Map(x => x.OwnerName, "LessorName");
        }
    }
}