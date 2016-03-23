namespace Phundus.Shop.Infrastructure.Persistence.Model
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Shop.Model;

    public class CartItemMap : ClassMap<CartItem>
    {
        public CartItemMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_CartItem");

            CompositeId(x => x.CartItemId).KeyProperty(kp => kp.Id, "CartItemGuid");
            Version(x => x.Version);

            References(x => x.Cart, "CartGuid");

            Map(x => x.Position, "Position");
            Map(x => x.Quantity, "Quantity");
            Map(x => x.From, "FromUtc").CustomType<UtcDateTimeType>();
            Map(x => x.To, "ToUtc").CustomType<UtcDateTimeType>();
            Map(x => x.LineText, "Article_Name");
            Map(x => x.UnitPrice, "Article_UnitPricePerWeek");
            Map(x => x.Days, "Days");
            Map(x => x.ItemTotal, "ItemTotal");

            Component(x => x.ArticleId, a =>
                a.Map(x => x.Id, "Article_ArticleGuid"));
            Component(x => x.ArticleShortId, a =>
                a.Map(x => x.Id, "Article_ArticleId"));
            Component(x => x.LessorId, a =>
                a.Map(x => x.Id, "Article_Owner_OwnerId"));
            Map(x => x.LessorName, "Article_Owner_Name");
            Component(x => x.StoreId, a =>
                a.Map(x => x.Id, "Article_StoreId"));
        }
    }
}