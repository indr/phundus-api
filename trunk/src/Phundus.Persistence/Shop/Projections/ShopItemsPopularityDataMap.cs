namespace Phundus.Persistence.Shop.Projections
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemsPopularityDataMap: ClassMap<ShopItemsPopularityData>
    {
        public ShopItemsPopularityDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Items_Popularity");

            Id(x => x.RowId).GeneratedBy.GuidComb();

            References(x => x.ShopItem, "ArticleId").UniqueKey("ArticleId_Month");
            
            Map(x => x.Month).Not.Nullable().UniqueKey("ArticleId_Month");
            Map(x => x.Value);
        }
    }
}