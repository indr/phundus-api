namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemImagesProjectionRowMap : ClassMap<ShopItemImagesProjectionRow>
    {
        public ShopItemImagesProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ItemImages");

            Id(x => x.ArticleGuid).GeneratedBy.Assigned().UniqueKey("ArticleGuid_FileName");
            Map(x => x.ArticleId);
            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleGuid_FileName"); ;
            Map(x => x.FileType);
        }
    }
}