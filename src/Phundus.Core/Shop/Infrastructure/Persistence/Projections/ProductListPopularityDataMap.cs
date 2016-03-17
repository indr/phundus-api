namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;

    public class ProductListPopularityDataMap : ClassMap<ProductListPopularityData>
    {
        public ProductListPopularityDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ProductList_Popularity");

            Id(x => x.RowId).GeneratedBy.GuidComb();

            References(x => x.ProductList, "ArticleId").UniqueKey("ArticleId_Month");

            Map(x => x.Month).Not.Nullable().UniqueKey("ArticleId_Month");
            Map(x => x.Value);
        }
    }
}