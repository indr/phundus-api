namespace Phundus.Persistence.Shop.Mappings
{
    using Extensions;
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemDataMap : ClassMap<ShopItemData>
    {
        public ShopItemDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Item");

            Id(x => x.ArticleGuid).GeneratedBy.Assigned();
            Map(x => x.ArticleId).Unique();
            Map(x => x.Brand);
            Map(x => x.Color);
            Map(x => x.Description).WithMaxSize();
            Map(x => x.MemberPrice);
            Map(x => x.Name);
            Map(x => x.LessorId, "LessorId");
            Map(x => x.LessorName, "LessorName");
            Map(x => x.LessorType, "LessorType");
            Map(x => x.StoreId);
            Map(x => x.StoreName);
            Map(x => x.Specification).WithMaxSize();
            Map(x => x.PublicPrice);

            HasMany(x => x.Documents).KeyColumn("ArticleGuid").ReadOnly().Inverse().ForeignKeyCascadeOnDelete();
            HasMany(x => x.Images).KeyColumn("ArticleGuid").ReadOnly().Inverse().ForeignKeyCascadeOnDelete();
        }
    }
}