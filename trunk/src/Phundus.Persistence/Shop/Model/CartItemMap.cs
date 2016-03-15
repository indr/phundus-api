namespace Phundus.Persistence.Shop.Model
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Model;

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
            Map(x => x.Days, "Days");
            Map(x => x.ItemTotal, "ItemTotal");

            Component(x => x.Article, m =>
            {
                m.Component(x => x.ArticleShortId, a =>
                    a.Map(x => x.Id, "Article_ArticleId"));
                m.Component(x => x.ArticleId, a =>
                    a.Map(x => x.Id, "Article_ArticleGuid"));
                m.Component(x => x.StoreId, a =>
                    a.Map(x => x.Id, "Article_StoreId"));

                m.Map(x => x.Name, "Article_Name");
                m.Map(x => x.Price, "Article_UnitPricePerWeek");
                m.Component(x => x.Lessor, a =>
                {
                    a.Component(x => x.LessorId, a2 =>
                        a2.Map(x => x.Id, "Article_Owner_OwnerId"));
                    a.Map(x => x.Name, "Article_Owner_Name");
                });
            });
        }
    }
}