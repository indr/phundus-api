namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemProjectionRowMap : ClassMap<ShopItemProjectionRow>
    {
        public ShopItemProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Item");

            Id(x => x.ArticleGuid).GeneratedBy.Assigned();
            Map(x => x.ArticleId).Unique();
            Map(x => x.Brand);
            Map(x => x.Color);
            Map(x => x.Description);
            Map(x => x.MemberPrice);
            Map(x => x.Name);
            Map(x => x.LessorId, "OwnerGuid");
            Map(x => x.LessorName, "OwnerName");
            Map(x => x.LessorType, "OwnerType");
            Map(x => x.Specification);
            Map(x => x.PublicPrice);

            HasMany(x => x.Documents).KeyColumn("ArticleGuid").ReadOnly();
            HasMany(x => x.Images).KeyColumn("ArticleGuid").ReadOnly();
        }
    }
}