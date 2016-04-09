namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using Common.Infrastructure.Persistence;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

    public class ProductDetailsDataMap : ClassMap<ProductDetailsData>
    {
        public ProductDetailsDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ProductDetails");

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
            Map(x => x.Brand);
            Map(x => x.Color);
            Map(x => x.Description).WithMaxSize();
            Map(x => x.Specification).WithMaxSize();
            Map(x => x.PublicPrice);
            Map(x => x.MemberPrice);

            HasMany(x => x.Documents).KeyColumn("ArticleId")
                .Inverse().Cascade.AllDeleteOrphan()
                .ForeignKeyCascadeOnDelete();

            HasMany(x => x.Images).KeyColumn("ArticleId")
                .Inverse().Cascade.AllDeleteOrphan()
                .ForeignKeyCascadeOnDelete();

            HasMany(x => x.Tags).Table("Es_Shop_ProductDetails_Tags").AsSet().Element("TagName", m => m.Type<string>()); 
        }
    }
}