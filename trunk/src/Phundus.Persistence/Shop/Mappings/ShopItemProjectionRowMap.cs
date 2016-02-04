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

            Id(x => x.RowId).GeneratedBy.GuidComb();
            Map(x => x.ArticleGuid).Unique();
            Map(x => x.ArticleId).Unique();
            Map(x => x.Description);
            Map(x => x.MemberPrice);
            Map(x => x.Name);
            Map(x => x.OwnerGuid);
            Map(x => x.OwnerName);
            Map(x => x.OwnerType);
            Map(x => x.Specification);
            Map(x => x.PublicPrice);
        }
    }
}