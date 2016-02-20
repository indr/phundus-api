namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemsSortByPopularityProjectionRowMap: ClassMap<ShopItemsSortByPopularityProjectionRow>
    {
        public ShopItemsSortByPopularityProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ShopItemsSortByPopularityProjection");

            Id(x => x.RowId).GeneratedBy.GuidComb();
            Map(x => x.ArticleId).Not.Nullable().UniqueKey("ArticleId_Month");
            Map(x => x.Month).Not.Nullable().UniqueKey("ArticleId_Month");
            Map(x => x.Value);
        }
    }
}