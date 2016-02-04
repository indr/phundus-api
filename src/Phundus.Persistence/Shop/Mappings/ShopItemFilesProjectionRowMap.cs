namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class ShopItemFilesProjectionRowMap : ClassMap<ShopItemFilesProjectionRow>
    {
        public ShopItemFilesProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_ItemFiles");

            Id(x => x.RowId).GeneratedBy.GuidComb();
            Map(x => x.ArticleGuid).UniqueKey("UC_Es_Shop_ItemFiles_ArticleGuid_FileName");
            Map(x => x.ArticleId);
            Map(x => x.FileLength);
            Map(x => x.FileName).UniqueKey("UC_Es_Shop_ItemFiles_ArticleGuid_FileName"); ;
            Map(x => x.FileType);
        }
    }
}