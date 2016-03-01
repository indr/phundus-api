namespace Phundus.Persistence.Shop.Projections
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Projections;

    public class ShopItemsDataMap : ClassMap<ShopItemsData>
    {
        public ShopItemsDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Items");

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