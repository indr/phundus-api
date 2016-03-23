namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

    public class ProductListDataMap : ClassMap<ProductListData>
    {
        public ProductListDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ProductList");

            Id(x => x.ArticleId).GeneratedBy.Assigned();
            Map(x => x.ArticleShortId).Unique();
            Map(x => x.CreatedAtUtc).CustomType<UtcDateTimeType>();

            Map(x => x.LessorId);
            Map(x => x.LessorType).CustomType<LessorType>();
            Map(x => x.LessorName);
            Map(x => x.LessorUrl);

            Map(x => x.StoreId);
            Map(x => x.StoreName);
            Map(x => x.StoreUrl);

            Map(x => x.Name);
            Map(x => x.PreviewImageFileName);
            Map(x => x.PublicPrice);
            Map(x => x.MemberPrice);

            HasMany(x => x.Popularities).KeyColumn("ArticleId")
                .Inverse().Cascade.AllDeleteOrphan()
                .ForeignKeyCascadeOnDelete();
        }
    }
}