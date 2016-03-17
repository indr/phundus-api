namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;

    public class ShopItemsPopularityDataMap : ClassMap<ProductPopularityData>
    {
        public ShopItemsPopularityDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Items_Popularity");

            Id(x => x.RowId).GeneratedBy.GuidComb();

            References(x => x.ProductList, "ArticleId").UniqueKey("ArticleId_Month");

            Map(x => x.Month).Not.Nullable().UniqueKey("ArticleId_Month");
            Map(x => x.Value);
        }
    }
}