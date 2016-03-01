namespace Phundus.Persistence.Shop.Projections
{
    using Extensions;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Projections;

    public class ShopItemDataMap : ClassMap<ShopItemData>
    {
        public ShopItemDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Item");

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
        }
    }
}