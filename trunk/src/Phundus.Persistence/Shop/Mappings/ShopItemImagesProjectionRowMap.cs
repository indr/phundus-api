namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemImagesProjectionRowMap : ClassMap<ShopItemImagesProjectionRow>
    {
        public ShopItemImagesProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Item_Images");

            Id(x => x.RowGuid).GeneratedBy.GuidComb();
            Map(x => x.ArticleGuid).UniqueKey("ArticleGuid_FileName");
            Map(x => x.ArticleId);
            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("ArticleGuid_FileName"); ;
            Map(x => x.FileType);
        }
    }
}